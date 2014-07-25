/*
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
 * in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License
 * is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions and limitations under
 * the License.
 */
/*
 * This code was generated by https://code.google.com/p/google-apis-client-generator/
 * (build: 2014-07-09 17:08:39 UTC)
 * on 2014-07-21 at 05:12:42 UTC 
 * Modify at your own risk.
 */

package com.google.samplesolutions.mobileassistant.checkinendpoint.model;

/**
 * Model definition for CheckIn.
 *
 * <p> This is the Java data model class that specifies how to parse/serialize into the JSON that is
 * transmitted over HTTP when working with the checkinendpoint. For a detailed explanation see:
 * <a href="http://code.google.com/p/google-http-java-client/wiki/JSON">http://code.google.com/p/google-http-java-client/wiki/JSON</a>
 * </p>
 *
 * @author Google, Inc.
 */
@SuppressWarnings("javadoc")
public final class CheckIn extends com.google.api.client.json.GenericJson {

  /**
   * The value may be {@code null}.
   */
  @com.google.api.client.util.Key
  private com.google.api.client.util.DateTime checkinDate;

  /**
   * The value may be {@code null}.
   */
  @com.google.api.client.util.Key
  private Key key;

  /**
   * The value may be {@code null}.
   */
  @com.google.api.client.util.Key
  private java.lang.String placeId;

  /**
   * The value may be {@code null}.
   */
  @com.google.api.client.util.Key
  private java.lang.String userEmail;

  /**
   * @return value or {@code null} for none
   */
  public com.google.api.client.util.DateTime getCheckinDate() {
    return checkinDate;
  }

  /**
   * @param checkinDate checkinDate or {@code null} for none
   */
  public CheckIn setCheckinDate(com.google.api.client.util.DateTime checkinDate) {
    this.checkinDate = checkinDate;
    return this;
  }

  /**
   * @return value or {@code null} for none
   */
  public Key getKey() {
    return key;
  }

  /**
   * @param key key or {@code null} for none
   */
  public CheckIn setKey(Key key) {
    this.key = key;
    return this;
  }

  /**
   * @return value or {@code null} for none
   */
  public java.lang.String getPlaceId() {
    return placeId;
  }

  /**
   * @param placeId placeId or {@code null} for none
   */
  public CheckIn setPlaceId(java.lang.String placeId) {
    this.placeId = placeId;
    return this;
  }

  /**
   * @return value or {@code null} for none
   */
  public java.lang.String getUserEmail() {
    return userEmail;
  }

  /**
   * @param userEmail userEmail or {@code null} for none
   */
  public CheckIn setUserEmail(java.lang.String userEmail) {
    this.userEmail = userEmail;
    return this;
  }

  @Override
  public CheckIn set(String fieldName, Object value) {
    return (CheckIn) super.set(fieldName, value);
  }

  @Override
  public CheckIn clone() {
    return (CheckIn) super.clone();
  }

}
