/*  
    Copyright (c) 2012 Microsoft Corporation.  All rights reserved. 
    Use of this sample source code is subject to the terms of the Microsoft license  
    agreement under which you licensed this sample source code and is provided AS-IS. 
    If you did not accept the terms of the license agreement, you are not authorized  
    to use this sample source code.  For the terms of the license, please see the  
    license agreement between you and Microsoft. 
   
    To see all Code Samples for Windows Phone, visit http://go.microsoft.com/fwlink/?LinkID=219604  
   
*/
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Media.Imaging;


namespace StarSightings.Helpers
{
    // This class handles uploading the file to the service. 
    internal class AsyncHttpPostHelper
    {
        public static void HttpUploadFile(string url, string file, string paramName, string contentType, Dictionary<string,string> nvc)
        {
            Console.WriteLine(string.Format("Uploading {0} to {1}", file, url));
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest httpWebRequest  = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest .ContentType = "multipart/form-data; boundary=" + boundary;
            httpWebRequest .Method = "POST";
            //wr.KeepAlive = true;
            //wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
#if (DEBUG)
            string auth = Constants.BASE_AUTH_USERNAME + ":" + Constants.BASE_AUTH_PASSWORD;
            string authString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
            httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Basic " + authString;
#endif
            
            //Stream rs = wr.GetRequestStream();
            httpWebRequest.BeginGetRequestStream((result) =>
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)result.AsyncState;
                    using (Stream requestStream = request.EndGetRequestStream(result))
                    {
                        string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                        foreach (string key in nvc.Keys)
                        {
                            requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                            string formitem = string.Format(formdataTemplate, key, nvc[key]);
                            byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                            requestStream.Write(formitembytes, 0, formitembytes.Length);
                        }
                        requestStream.Write(boundarybytes, 0, boundarybytes.Length);
                        
                        string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                        string header = string.Format(headerTemplate, paramName, file, contentType);
                        byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                        requestStream.Write(headerbytes, 0, headerbytes.Length);
                        
                        using (MemoryStream stream = new MemoryStream())
                        {
                            App.ViewModel.WriteableSelectedBitmap.SaveJpeg(stream, App.ViewModel.WriteableSelectedBitmap.PixelWidth, App.ViewModel.WriteableSelectedBitmap.PixelHeight, 0, 100);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] buffer = new byte[4096];
                            int bytesRead = 0;
                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                requestStream.Write(buffer, 0, bytesRead);
                            }
                            stream.Close();
                        }
                        
                        byte[] trailer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                        requestStream.Write(trailer, 0, trailer.Length);

                        requestStream.Close();
                    }

                    request.BeginGetResponse(a =>
                    {
                        try
                        {
                            var response = request.EndGetResponse(a);
                            var responseStream = response.GetResponseStream();
                            using (var sr = new StreamReader(responseStream))
                                Console.WriteLine(string.Format("File uploaded, server response is: {0}", sr.ReadToEnd()));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.StackTrace);
                        }
                    }, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

            }, httpWebRequest);
            

            

            /*
            FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[4096];
            int bytesRead = 0;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                rs.Write(buffer, 0, bytesRead);
            }
            fileStream.Close();
            */
            

            
            
            /*
            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);
                Console.WriteLine(string.Format("File uploaded, server response is: {0}", reader2.ReadToEnd()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error uploading file", ex);
                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
             * */
        }

        internal RequestState requestState;
        internal HttpWebRequest request;
        // This delegate references the method passed by the ServiceUploadHelper. It 
        // allows the logic for finding the file to upload and uploading the file to 
        // the service be separated. It is set in the BeginSend() method. 
        internal delegate void OnHttpPostCompleteDelegate(Picture picture);
        OnHttpPostCompleteDelegate httpPostCompleteCallbackDelegate;

        // TODO: Adjust as needed. 
        // The ConstantStrings class contains most of the settings that the 
        // developer should modify. A full description of each component is 
        // contained below. 
        public class ConstantStrings
        {
            // apiUri: the URI to post the content. The string stored in the 
            // settingsKey is appended to the apiUri when uploading. 
            public static string apiUri = "https://api.contoso.com/files?access_token=";
            public static string settingsKey = "AccessToken";
            // contentType: Set in the HTTP request 
            public static string contentType = "multipart/form-data; boundary=A300x";
            public static string method = "POST";
            // headerString: sent before uploading the file, adjust as necessary 
            public static string headerString = "--A300x\r\nContent-Disposition: form-data; name=\"file\"; filename=\"{0}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
            // footerString: sent after the file has been submitted 
            public static string footerString = "\r\n--A300x--";
            // albumName: Determines which album from which to upload photos. 
            public static string albumName = "Camera Roll";
        }

        // The RequestState is passed between the callbacks to keep track of the image being uploaded. 
        internal class RequestState
        {
            public HttpWebRequest request; // the HTTP request 
            public Picture picture; // picture being uploaded             
            public Stream source; // source stream, taken from the Picture 
            public Stream destination; // destination stream to the service 
            public byte[] postData = null; // buffers the data to send, set in the RequestState constructor 
            public byte[] buffer; // buffer for other data. 
            public byte[] headerBytes; // buffer for bytes sent in the header 
            public byte[] footerBytes = Encoding.UTF8.GetBytes(ConstantStrings.footerString);
            public int bytesWritten; // tracks the number of bytes sent. Must match the ContentLength 

            public long ContentLength;
            public RequestState(Picture p, Uri u)
            {
                picture = p;
                //headerBytes = System.Text.Encoding.UTF8.GetBytes(String.Format(ConstantStrings.headerString, HttpUtility.UrlEncode(picture.Name)));
                headerBytes = System.Text.Encoding.UTF8.GetBytes(String.Format(ConstantStrings.headerString, HttpUtility.UrlEncode("wpload_"+DateTime.Now.ToString())));
                request = (HttpWebRequest)WebRequest.Create(u);
                request.Method = ConstantStrings.method;                
                //request.AllowWriteStreamBuffering = false; // important, allows data to be sent immediately, avoiding OOM exceptions 
                request.ContentType = ConstantStrings.contentType;

                //ContentLength = picture.GetImage().Length + headerBytes.Length + footerBytes.Length;  
                source = new MemoryStream();
                App.ViewModel.WriteableSelectedBitmap.SaveJpeg(source,App.ViewModel.WriteableSelectedBitmap.PixelWidth, App.ViewModel.WriteableSelectedBitmap.PixelHeight,0,100);
                ContentLength = source.Length + headerBytes.Length + footerBytes.Length;                
                request.Headers[HttpRequestHeader.ContentLength] = ContentLength.ToString();
                
                //request.ContentLength = picture.GetImage().Length + headerBytes.Length + footerBytes.Length;
#if (DEBUG)
                string auth = Constants.BASE_AUTH_USERNAME + ":" + Constants.BASE_AUTH_PASSWORD;
                string authString = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
                request.Headers[HttpRequestHeader.Authorization] = "Basic " + authString;
#endif
                
                
                bytesWritten = 0;
                postData = new byte[4 * 1024];
            }
        }



        //  BeginSend() starts the asynchronous operation by calling 
        //  GetRequestStreamCallback(). 

        //  Once the request stream has been received, GetRequestStreamCallback() 
        //  ends the operation by saving the stream in the RequestState and then 
        //  calls SendNextChunk() to post the data to the stream. 

        //  SendNextChunk() determines whether to send the header, footer, or body of 
        //  the POST request by comparing the number of bytes sent and the 
        //  ContentLength set earlier (in the RequestState constructor). 

        //  Note that it's important to handle reads and writes asynchronously to 
        //  improve the performance of the background agent, especially if the image 
        //  is large. If SendNextChunk() elects to send the header or footer to the 
        //  destination stream then GetStreamWriteCallback() will post to the stream 
        //  the existing header or footer byte array stored in the RequestState. 

        //  If SendNextChunk() elects to send the body to the destination stream, then 
        //  GetStreamReadCallback() will asynchronously read the image, 4 kB at a 
        //  time (see postData[] in RequestState), and write those 4 kB to the 
        //  destination stream by calling GetStreamWriteCallback(). 

        //  Once everything has been sent to the destination stream, SendNextChunk() 
        //  calls RespCallback() to asynchronously get the response from the service. 

        //  RespCallback() ends the operation, closes the stream objects (optionally 
        //  saving the response if necessary), and calls EndSend() to clean up the 
        //  flow. 

        //  EndSend() returns to the calling method, indicating that the next picture 
        //  can be sent. 

        internal void BeginSend(OnHttpPostCompleteDelegate callbackDelegate)
        {
            try
            {
                httpPostCompleteCallbackDelegate = callbackDelegate;
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), requestState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        internal void EndSend(Picture p)
        {
            httpPostCompleteCallbackDelegate(p); // End the upload flow for this picture. 
        }

        public AsyncHttpPostHelper(Picture p, Uri u)
        {
            requestState = new RequestState(p, u);
            request = requestState.request;
        }

        // The comparisons here must be processed in the listed order to ensure that the 
        // file is uploaded correctly. 
        private void SendNextChunk(RequestState rs)
        {
            if (rs.bytesWritten == 0) // send header 
            {
                rs.buffer = rs.headerBytes;
                rs.destination.BeginWrite(rs.buffer, 0, rs.buffer.Length, new AsyncCallback(GetStreamWriteCallback), rs);
                rs.destination.Flush();
                rs.bytesWritten += rs.buffer.Length;
            }
            //else if (rs.bytesWritten == rs.request.ContentLength - rs.footerBytes.Length) // send footer 
            else if (rs.bytesWritten == rs.ContentLength - rs.footerBytes.Length) // send footer 
            {
                rs.buffer = rs.footerBytes;
                rs.destination.BeginWrite(rs.buffer, 0, rs.buffer.Length, new AsyncCallback(GetStreamWriteCallback), rs);
                rs.destination.Flush();
                rs.bytesWritten += rs.buffer.Length;
            }
            //else if (rs.bytesWritten < rs.request.ContentLength) // send body 
            else if (rs.bytesWritten < rs.ContentLength) // send body 
            {
                rs.buffer = rs.postData;
                rs.source.BeginRead(rs.buffer, 0, rs.buffer.Length, new AsyncCallback(GetStreamReadCallback), rs);
            }
            //else if (rs.bytesWritten == rs.request.ContentLength) // stop sending 
            else if (rs.bytesWritten == rs.ContentLength) // stop sending 
            {
                rs.destination.Close();
                rs.bytesWritten = 0; // reset the bytes written counter 
                rs.request.BeginGetResponse(new AsyncCallback(RespCallback), rs);
            }
            else
            {
                Console.WriteLine("Error.");
            }
        }

        // Reads the source stream and starts writing to the destination stream. 
        private void GetStreamReadCallback(IAsyncResult ar)
        {
            RequestState requestState = (RequestState)ar.AsyncState;
            int bytesRead = requestState.source.EndRead(ar);
            requestState.bytesWritten += bytesRead;
            try
            {
                requestState.destination.BeginWrite(requestState.buffer, 0, bytesRead, new AsyncCallback(GetStreamWriteCallback), requestState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        // Writes to the destination stream. 
        private void GetStreamWriteCallback(IAsyncResult ar)
        {
            try
            {
                RequestState requestState = (RequestState)ar.AsyncState;
                requestState.destination.Flush();
                requestState.destination.EndWrite(ar);
                SendNextChunk(requestState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        // Ends the operation, saves the request stream, and prepares the next chunk 
        // to be sent. 
        private void GetRequestStreamCallback(IAsyncResult ar)
        {
            try
            {
                RequestState requestState = (RequestState)ar.AsyncState;
                //requestState.source = requestState.picture.GetImage();
                requestState.source = new MemoryStream();
                
                App.ViewModel.WriteableSelectedBitmap.SaveJpeg(requestState.source,App.ViewModel.WriteableSelectedBitmap.PixelWidth, App.ViewModel.WriteableSelectedBitmap.PixelHeight,0,100);
                
                requestState.destination = requestState.request.EndGetRequestStream(ar);
                SendNextChunk(requestState);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        // Reads the response from the server. 
        private void RespCallback(IAsyncResult ar)
        {
            try
            {
                RequestState requestState = (RequestState)ar.AsyncState;

                HttpWebResponse objHttpWebResponse = (HttpWebResponse)requestState.request.EndGetResponse(ar);
                Stream objStreamResponse = objHttpWebResponse.GetResponseStream();
                StreamReader objStreamReader = new StreamReader(objStreamResponse);

                // Response string may be used by developer. 
                string responseString = objStreamReader.ReadToEnd();

                // Close the stream object 
                objStreamResponse.Close();
                objStreamReader.Close();
                objHttpWebResponse.Close();

                // Notify the calling method that the upload has completed for this 
                // picture. 
                EndSend(requestState.picture);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}