import java.io.BufferedReader;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.Scanner;


public class IOTest {
    public static void main(String[] args) throws Exception {
        //copyBytes();
        //copyLines();
        scanTest();
    }

    private static void scanTest() throws IOException {
        Scanner sc = null;
        PrintWriter pw = null;

        try {
            sc = new Scanner(new BufferedReader(new FileReader("testdata")));
            pw = new PrintWriter(new FileWriter("outdata"));
            while (sc.hasNext()) {
                pw.print(sc.next());
            }
        } finally {
            if (sc != null)
                sc.close();
            if (pw != null)
                pw.close();

        }
    }

    private static void copyLines() throws IOException {
        BufferedReader br = null;
        PrintWriter pw = null;

        try {
            br = new BufferedReader(new FileReader("testdata"));
            pw = new PrintWriter(new FileWriter("outdata"));
            String l;
            while ((l = br.readLine()) != null)
                pw.println(l);
        } finally {
            if (br != null)
                br.close();
            if (pw != null)
                pw.close();
        }

    }

    private static void copyBytes() throws IOException{
        FileInputStream in = null;
        FileOutputStream out = null;

        try {
            in = new FileInputStream("testdata");
            out = new FileOutputStream("outdata");
            int c;

            while ((c=in.read())!=-1) {
                out.write(c);
            }
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            if (in != null)
                in.close();
            if (out != null)
                out.close();
        }
    }


}
