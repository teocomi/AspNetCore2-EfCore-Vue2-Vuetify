using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace AspNetCore.Sample.DesktopUtils
{
    public static class GoogleAnalyticsApi
    {
        /*
        *   Edited from: https://gist.github.com/tylerbhughes/f164609c2405056c637e
        *   Credit for the Track function and the Enum HitType goes to 0liver (https://gist.github.com/0liver/11229128)
        *   Credit goes to spyriadis (http://www.spyriadis.net/2014/07/google-analytics-measurement-protocol-track-events-c/)
        *       for the idea of putting the values for each tracking method in its own function
        *
        *   Documentation of the Google Analytics Measurement Protocol can be found at:
        *   https://developers.google.com/analytics/devguides/collection/protocol/v1/devguide
        */

        private static string endpoint = "http://www.google-analytics.com/collect";
        private static string googleVersion = "1";
        private static string googleTrackingID = "UA-120589378-1";
        private static string googleClientID = "FB256118-F34E-47C5-9282-F012D90E01A7";

        public static void TrackEvent(string category, string action, string label, int? value = null)
        {
            if (string.IsNullOrEmpty(category)) throw new ArgumentNullException("category");
            if (string.IsNullOrEmpty(action)) throw new ArgumentNullException("action");

            var values = DefaultValues;

            values.Add("t", HitType.@event.ToString());             // Event hit type
            values.Add("ec", category);                             // Event Category. Required.
            values.Add("ea", action);                               // Event Action. Required.
            if (label != null) values.Add("el", label);             // Event label.
            if (value != null) values.Add("ev", value.ToString());  // Event value.

            Track(values);
        }

        public static void TrackPageview(string category, string action, string label, int? value = null)
        {
            if (string.IsNullOrEmpty(category)) throw new ArgumentNullException("category");
            if (string.IsNullOrEmpty(action)) throw new ArgumentNullException("action");

            var values = DefaultValues;

            values.Add("t", HitType.@pageview.ToString());          // Event hit type
            values.Add("ec", category);                             // Event Category. Required.
            values.Add("ea", action);                               // Event Action. Required.
            if (label != null) values.Add("el", label);             // Event label.
            if (value != null) values.Add("ev", value.ToString());  // Event value.

            Track(values);
        }

        public static async void Track(Dictionary<string, string> values)
        {
            var request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Method = "POST";
            request.KeepAlive = false;

            var postDataString = values
                .Aggregate("", (data, next) => string.Format("{0}&{1}={2}", data, next.Key,
                                                             HttpUtility.UrlEncode(next.Value)))
                .TrimEnd('&');

            // set the Content-Length header to the correct value
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataString);

            // write the request body to the request
            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(postDataString);
            }

            try
            {
                // Send the response to the server
                var webResponse = await request.GetResponseAsync();

                if ((webResponse as HttpWebResponse).StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Google Analytics tracking did not return OK 200");
                }

                webResponse.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private enum HitType
        {
            // ReSharper disable InconsistentNaming
            @event,
            @pageview,
            // ReSharper restore InconsistentNaming
        }

        private static Dictionary<string, string> DefaultValues
        {
            get
            {
                var data = new Dictionary<string, string>();
                data.Add("v", googleVersion);         // The protocol version. The value should be 1.
                data.Add("tid", googleTrackingID);    // Tracking ID / Web property / Property ID.
                data.Add("cid", googleClientID);      // Anonymous Client ID (must be unique).
                return data;
            }
        }
    }
}
