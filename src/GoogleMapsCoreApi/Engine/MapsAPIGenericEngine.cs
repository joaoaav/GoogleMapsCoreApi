using GoogleMapsCoreApi.Entities.Common;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

namespace GoogleMapsCoreApi.Engine
{
    public delegate Uri UriCreatedDelegate(Uri uri);
    public delegate void RawResponseReciviedDelegate(byte[] data);

    public abstract class MapsAPIGenericEngine<TRequest, TResponse>
        where TRequest : MapsBaseRequest, new()
        where TResponse : IResponseFor<TRequest>
    {
        internal static event UriCreatedDelegate OnUriCreated;
        internal static event RawResponseReciviedDelegate OnRawResponseRecivied;

        /// <summary>
        /// Determines the maximum number of concurrent HTTP connections to open to this engine's host address. The default value is 2 connections.
        /// </summary>
        /// <remarks>
        /// This value is determined by the ServicePointManager and is shared across other engines that use the same host address.
        /// </remarks>
        /*public static int HttpConnectionLimit
        {
            get { return HttpServicePoint.ConnectionLimit; }
            set { HttpServicePoint.ConnectionLimit = value; }
        }*/

        /// <summary>
        /// Determines the maximum number of concurrent HTTPS connections to open to this engine's host address. The default value is 2 connections.
        /// </summary>
        /// <remarks>
        /// This value is determined by the ServicePointManager and is shared across other engines that use the same host address.
        /// </remarks>
        /*public static int HttpsConnectionLimit
        {
            get { return HttpsServicePoint.ConnectionLimit; }
            set { HttpsServicePoint.ConnectionLimit = value; }
        }*/

        //private static ServicePoint HttpServicePoint { get; set; }
        //private static ServicePoint HttpsServicePoint { get; set; }
        internal static TimeSpan DefaultTimeout = TimeSpan.FromSeconds(100);

        private const string AuthenticationFailedMessage =
            "The request to Google API failed with HTTP error '(403) Forbidden', which usually indicates that the provided client ID or signing key is invalid or expired.";

        static MapsAPIGenericEngine()
        {
            var baseUrl = new TRequest().BaseUrl;
            //HttpServicePoint = ServicePointManager.FindServicePoint(new Uri("http://" + baseUrl));
            //HttpsServicePoint = ServicePointManager.FindServicePoint(new Uri("https://" + baseUrl));
        }

        protected internal static TResponse QueryGoogleAPI(TRequest request, TimeSpan timeout)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var uri = request.GetUri();
            if (OnUriCreated != null)
            {
                uri = OnUriCreated(uri);
            }

            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GoogleMapsCoreApi", "1.0"));
            httpClient.Timeout = timeout;
            var response = httpClient.GetAsync(uri).Result;

            string stringData = response.Content.ReadAsStringAsync().Result;

            var data = JsonConvert.DeserializeObject<TResponse>(stringData);

            return data;
        }

        protected static IAsyncResult BeginQueryGoogleAPI(TRequest request, AsyncCallback asyncCallback, object state)
        {
            // Must use TaskCompletionSource because in .NET 4.0 there's no overload of ContinueWith that accepts a state object (used in IAsyncResult).
            // Such overloads have been added in .NET 4.5, so this can be removed if/when the project is promoted to that version.
            // An example of such an added overload can be found at: http://msdn.microsoft.com/en-us/library/hh160386.aspx

            var completionSource = new TaskCompletionSource<TResponse>(state);
            QueryGoogleAPIAsync(request).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    completionSource.SetException(t.Exception.InnerException);
                else if (t.IsCanceled)
                    completionSource.SetCanceled();
                else
                    completionSource.SetResult(t.Result);

                asyncCallback(completionSource.Task);
            }, TaskContinuationOptions.ExecuteSynchronously);

            return completionSource.Task;
        }

        protected static TResponse EndQueryGoogleAPI(IAsyncResult asyncResult)
        {
            return ((Task<TResponse>)asyncResult).Result;
        }

        protected static Task<TResponse> QueryGoogleAPIAsync(TRequest request)
        {
            return QueryGoogleAPIAsync(request, TimeSpan.FromMilliseconds(Timeout.Infinite), CancellationToken.None);
        }

        protected internal static Task<TResponse> QueryGoogleAPIAsync(TRequest request, TimeSpan timeout, CancellationToken token = default(CancellationToken))
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var completionSource = new TaskCompletionSource<TResponse>();

            var uri = request.GetUri();
            if (OnUriCreated != null)
            {
                uri = OnUriCreated(uri);
            }

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GoogleMapsCoreApi", "1.0"));
            httpClient.Timeout = timeout;
            var data = httpClient.GetByteArrayAsync(uri)
                .ContinueWith(t => DownloadDataComplete(t, completionSource), TaskContinuationOptions.ExecuteSynchronously);
            
            return completionSource.Task;
        }

        private static void DownloadDataComplete(Task<byte[]> task, TaskCompletionSource<TResponse> completionSource)
        {
            if (task.IsCanceled)
            {
                completionSource.SetCanceled();
            }
            else if (task.IsFaulted)
            {
                completionSource.SetException(task.Exception.InnerException);
            }
            else
            {
                OnRawResponseRecivied?.Invoke(task.Result);
                completionSource.SetResult(Deserialize(task.Result));
            }
        }

        private static TResponse Deserialize(byte[] serializedObject)
        {
            var serializer = new DataContractJsonSerializer(typeof(TResponse));
            var stream = new MemoryStream(serializedObject, false);
            return (TResponse)serializer.ReadObject(stream);
        }

        /*private static bool IndicatesAuthenticationFailed(Exception ex)
        {
            var webException = ex as WebException;

            return webException != null &&
                   webException.Status == WebExceptionStatus.ProtocolError &&
                   ((HttpWebResponse)webException.Response).StatusCode == HttpStatusCode.Forbidden;
        }*/
    }
}
