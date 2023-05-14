namespace Financial_Candlestick_Patterns
{
    public class Scrapper
    {
        public Task<string> GetHtml(string url)
        {
            return GetHtmlDocumentAsync(url);
        }

        public async Task<string> GetHtmlDocumentAsync(string url)
        {
            string html = string.Empty;
            var httpClient = GetNewClient();

            using (HttpResponseMessage response = await httpClient.GetAsync(url))
            {
                using (HttpContent content = response.Content)
                {
                    html = content.ReadAsStringAsync().Result;
                }
            }

            return html;
        }

        private HttpClient GetNewClient()
        {
            return new HttpClient();
        }
    }
}
