
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PHVN_WS_CORE.SHARED.Extensions
{
    public static class CollectionExtensions
    {
        public static bool NotNullOrEmpty<T>(this IEnumerable<T> list)
            => list != null && list.Any();

        public static string ToHtmlTable<T>(this IEnumerable<T> list)
        {
            var type = typeof(T);
            var props = type.GetProperties();
            var html = new StringBuilder("<table border='1px' cellpadding='5' cellspacing='0' style='border: solid 1px Silver; font-size: x-small;>");

            //Header
            html.Append("<thead><tr>");
            foreach (var p in props)
                html.Append("<th style='border: solid 1px Silver;' align='left' valign='top'>" + p.Name + "</th>");
            html.Append("</tr></thead>");

            //Body
            html.Append("<tbody>");
            foreach (var e in list)
            {
                html.Append("<tr align='left' valign='top'>");
                props.Select(s => s.GetValue(e)).ToList().ForEach(p => {
                    html.Append("<td style='border: solid 1px Silver;' align='left' valign='top'>" + p + "</td>");
                });
                html.Append("</tr>");
            }

            html.Append("</tbody>");
            html.Append("</table>");
            return html.ToString();
        }        

    }
}
