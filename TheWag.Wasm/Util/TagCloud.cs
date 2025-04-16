using System.Data;
using System.Text;
using System.Web;

namespace TheWag.Wasm.Util
{
    public class TagCloud
    {
        public static string GetCloud(IDictionary<string,int> tagData, string cloudTemplate)
        {

            StringBuilder outputBuffer = new StringBuilder();
            double max = 0;
            double min = 0;

            // Use Compute to get the min and max counts
            min = tagData.Values.Min();
            max = tagData.Values.Max();
            //double.TryParse(tagData.Compute("min(count)", null).ToString(), out min);
            //double.TryParse(tagData.Compute("max(count)", null).ToString(), out max);

            foreach (var row in tagData)
            {
                double weightPercent = (row.Value / max) * 100;
                int weight = 0;

                if (weightPercent >= 99)
                {
                    //heaviest
                    weight = 1;
                }
                else if (weightPercent >= 70)
                {
                    weight = 2;
                }
                else if (weightPercent >= 40)
                {
                    weight = 3;
                }
                else if (weightPercent >= 20)
                {
                    weight = 4;
                }
                else if (weightPercent >= 3)
                {
                    //weakest
                    weight = 5;
                }
                else
                {
                    // use this to filter out all low hitters
                    weight = 0;
                }

                //if (weight > 0)
                //    outputBuffer.Append(cloudTemplate.Replace("$weight$",
                //        weight.ToString()).Replace("$tag$",
                //        row.Key.ToString()).Replace("$urlencodetag$", HttpUtility.UrlEncode(row["tag"].ToString())));

                if (weight > 0)
                    outputBuffer.Append(cloudTemplate.Replace("$weight$",
                        weight.ToString()).Replace("$tag$",
                        row.Key).Replace("$urlencodetag$", HttpUtility.UrlEncode(row.Key)));
            }

            return outputBuffer.ToString();

        }
    }
}
