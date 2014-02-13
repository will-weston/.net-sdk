﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using System.IO;
using System.Configuration;
using CTCT.Exceptions;

namespace CTCT.Util
{
    /// <summary>
    /// Class implementation of REST client.
    /// </summary>
    public class RestClient : IRestClient
    {
        /// <summary>
        /// Make an Http GET request.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="accessToken">Constant Contact OAuth2 access token</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <returns>The response body, http info, and error (if one exists).</returns>
        public CUrlResponse Get(string url, string accessToken, string apiKey)
        {
            return this.HttpRequest(url, WebRequestMethods.Http.Get, accessToken, apiKey, null, null);
        }

        /// <summary>
        /// Make an Http POST request.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="accessToken">Constant Contact OAuth2 access token</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="data">Data to send with request.</param>
        /// <returns>The response body, http info, and error (if one exists).</returns>
        public CUrlResponse Post(string url, string accessToken, string apiKey, string data)
        {
			byte[] bytes = null;

			if(!string.IsNullOrEmpty(data))
			{
				// Convert the request contents to a byte array
				bytes = System.Text.Encoding.UTF8.GetBytes(data);
			}

            return this.HttpRequest(url, WebRequestMethods.Http.Post, accessToken, apiKey, bytes, null);
        }

		/// <summary>
		/// Make an HTTP Post Multipart request.
		/// </summary>
		/// <param name="url">Request URL.</param>
        /// <param name="accessToken">Constant Contact OAuth2 access token</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="data">Data to send with request.</param>
        /// <returns>The response body, http info, and error (if one exists).</returns>
		public CUrlResponse PostMultipart(string url, string accessToken, string apiKey, byte[] data)
        {
            return this.HttpRequest(url, WebRequestMethods.Http.Post, accessToken, apiKey, data, true);
        }

        /// <summary>
        /// Make an Http PUT request.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="accessToken">Constant Contact OAuth2 access token</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <param name="data">Data to send with request.</param>
        /// <returns>The response body, http info, and error (if one exists).</returns>
        public CUrlResponse Put(string url, string accessToken, string apiKey, string data)
        {
			byte[] bytes = null;

			if(!string.IsNullOrEmpty(data))
			{
				// Convert the request contents to a byte array 
				bytes = System.Text.Encoding.UTF8.GetBytes(data);
			}

            return this.HttpRequest(url, WebRequestMethods.Http.Put, accessToken, apiKey, bytes, null);
        }

        /// <summary>
        /// Make an Http DELETE request.
        /// </summary>
        /// <param name="url">Request URL.</param>
        /// <param name="accessToken">Constant Contact OAuth2 access token</param>
        /// <param name="apiKey">The API key for the application</param>
        /// <returns>The response body, http info, and error (if one exists).</returns>
        public CUrlResponse Delete(string url, string accessToken, string apiKey)
        {
            return this.HttpRequest(url, "DELETE", accessToken, apiKey, null, null);
        }

        private CUrlResponse HttpRequest(string url, string method, string accessToken, string apiKey, byte[] data, bool? isMultipart)
        {
            // Initialize the response
            CUrlResponse urlResponse = new CUrlResponse();

            var address = url;

            if (!string.IsNullOrEmpty(apiKey))
            {
                address = string.Format("{0}{1}api_key={2}", url, url.Contains("?") ? "&" : "?", apiKey);
            }

            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
                                                                                             
            request.Method = method;
			request.Accept = "application/json";

			if(isMultipart.HasValue && isMultipart.Value)
			{
				request.ContentType = "multipart/form-data; boundary=" + MultipartBuilder.MULTIPART_BOUNDARY;
			}
			else
			{
				request.ContentType = "application/json";			
			}
          
            // Add token as HTTP header
            request.Headers.Add("Authorization", "Bearer " + accessToken);

            if (data != null)
            {
                using (var stream = request.GetRequestStream())
                    stream.Write(data, 0, data.Length);
            }

            // Now try to send the request
            try
            {
                using (var response = request.GetResponse())
                {
                    var httpReponse = response as HttpWebResponse;
                    // Expect the unexpected
                    if (request.HaveResponse == true && httpReponse == null)
                        throw new WebException("Response was not returned or is null");

                    foreach (string header in httpReponse.Headers.AllKeys)
                    {
                        urlResponse.Headers.Add(header, httpReponse.GetResponseHeader(header));
                    }

                    urlResponse.StatusCode = httpReponse.StatusCode;
                    if (httpReponse.StatusCode != HttpStatusCode.OK)
                    {
                        urlResponse.IsError = true;
                        using (var stream = response.GetResponseStream())
                            urlResponse.Info = (IList<CUrlRequestError>)CUrlRequestError.FromJSON(stream, typeof(IList<CUrlRequestError>));
                    }
                    else
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                            urlResponse.Body = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException e)
            {
                urlResponse.IsError = true;
                if (e.Response != null && e.Response is HttpWebResponse)
                {
                    var response = (HttpWebResponse)e.Response;
                    urlResponse.StatusCode = response.StatusCode;
                    using (var stream = response.GetResponseStream())
                        urlResponse.Info = (IList<CUrlRequestError>)CUrlRequestError.FromJSON(stream, typeof(IList<CUrlRequestError>));
                }
            }

            //This is dumb.
            if (urlResponse.IsError)
                throw new CtctException(urlResponse.GetErrorMessage(), urlResponse.Info.ToArray(), urlResponse.StatusCode);

            return urlResponse;
        }
    }
}
