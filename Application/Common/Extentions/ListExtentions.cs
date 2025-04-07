using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Extentions
{
    public static class ListExtentions
    {
        public static T ShallowCopy<T>(this T @this) where T : class
        {
            if (@this == null)
            {
                return null;
            }
            var cloned = @this.GetType().GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(@this, null) as T;
            return cloned;
        }
        public static T DeepCopy<T>(this T obj)
        {
            // The same implementation as before
            if (obj == null)
            {
                return default(T);
            }

            Type type = obj.GetType();

            if (type == typeof(IFormFile))
            {
                return default(T); // Return null instead of creating a copy
            }

            if (type.IsValueType || type == typeof(string))
            {
                return obj;
            }

            if (type.IsArray)
            {
                Type elementType = type.GetElementType();
                var array = obj as Array;
                Array copied = Array.CreateInstance(elementType, array.Length);
                for (int i = 0; i < array.Length; i++)
                {
                    copied.SetValue(DeepCopy(array.GetValue(i)), i);
                }
                return (T)Convert.ChangeType(copied, obj.GetType());
            }


            object result = Activator.CreateInstance(obj.GetType());
            foreach (FieldInfo field in type.GetFields(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                object fieldValue = field.GetValue(obj);
                if (fieldValue == null)
                {
                    continue;
                }

                field.SetValue(result, DeepCopy(fieldValue));
            }
            return (T)result;



        }
        public static bool IsPropertyUnique<T, TProp>(this List<T> list, Func<T, TProp> selector)
        {
            var propertyValues = new HashSet<TProp>();
            foreach (var item in list)
            {
                var propertyValue = selector(item);
                if (propertyValues.Contains(propertyValue))
                {
                    return false;
                }
                propertyValues.Add(propertyValue);
            }
            return true;
        }

        public static string ExportToExcel<T>(this List<T> data, string? sheetName = "Sheet1")
        {
            List<string> sumColumns = new List<string>();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add(sheetName);



                // Get properties with Description attributes
                var properties = typeof(T).GetProperties()
                    .Where(p => Attribute.IsDefined(p, typeof(DescriptionAttribute)))
                    .ToList();

                // Add headers based on Description attributes
                for (int i = 0; i < properties.Count; i++)
                {
                    var descriptionAttribute = properties[i].GetCustomAttribute<DescriptionAttribute>();
                    var header = descriptionAttribute != null ? descriptionAttribute.Description : properties[i].Name;
                    worksheet.Cell(1, i + 1).Value = header;
                }

                // Add data rows
                for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
                {
                    var row = data[rowIndex];
                    for (int colIndex = 0; colIndex < properties.Count; colIndex++)
                    {
                        var value = properties[colIndex].GetValue(row);
                        var cell = worksheet.Cell(rowIndex + 2, colIndex + 1);
                        cell.Value = XLCellValue.FromObject(value);

                        // Apply currency format if this column is in the currencyColumns list
                        var descriptionAttribute = properties[colIndex].GetCustomAttribute<DescriptionAttribute>();
                        var header = descriptionAttribute != null ? descriptionAttribute.Description : properties[colIndex].Name;

                        if (value is decimal or double)
                        {
                            sumColumns.Add(header);
                            cell.Style.NumberFormat.Format = "#,##0.0#######"; // Currency format with thousands separator
                        }
                    }
                }

                worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.RangeUsed().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.RightToLeft = true;

                // Format headers (optional, e.g., bold)
                var headerStyle = workbook.Style;
                headerStyle.Font.Bold = true;
                headerStyle.Font.FontSize = 16;
                headerStyle.Fill.BackgroundColor = XLColor.BlueGray;

                worksheet.Row(1).Style = headerStyle;

                if (sumColumns != null)
                {
                    int totalRow = data.Count + 2;
                    worksheet.Cell(totalRow, 1).Value = "مجموع"; // Label for totals row

                    //change the total row color
                    var footerStyle = workbook.Style;
                    footerStyle.Font.Bold = true;
                    footerStyle.Font.FontSize = 15;
                    footerStyle.Fill.BackgroundColor = XLColor.FromHtml("#FFFFCC");
                    footerStyle.Font.FontColor = XLColor.CoolBlack;
                    worksheet.Row(totalRow).Style = footerStyle; // Label for totals row


                    for (int colIndex = 0; colIndex < properties.Count; colIndex++)
                    {
                        var descriptionAttribute = properties[colIndex].GetCustomAttribute<DescriptionAttribute>();
                        var header = descriptionAttribute != null ? descriptionAttribute.Description : properties[colIndex].Name;

                        if (sumColumns.Contains(header))
                        {
                            // Insert sum formula in the last row of the column
                            worksheet.Cell(totalRow, colIndex + 1).FormulaR1C1 = $"SUM(R2C{colIndex + 1}:R{totalRow - 1}C{colIndex + 1})";
                            worksheet.Cell(totalRow, colIndex + 1).Style.Font.Bold = true;
                            worksheet.Cell(totalRow, colIndex + 1).Style.NumberFormat.Format = "#,##0.0#######"; // Assuming prices are in currency format
                        }
                    }
                }

                worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.RangeUsed().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.RightToLeft = true;

                worksheet.Columns().AdjustToContents();

                // Save the workbook to the specified file path
                return ConvertFileToString(workbook);
            }
        }

        private static string ConvertFileToString(XLWorkbook workbook)
        {
            string file = "";

            if (workbook != null)
            {
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    file = Convert.ToBase64String(content);
                }
            }

            return file;
        }


    }
}
