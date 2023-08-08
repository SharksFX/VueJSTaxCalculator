using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection.Metadata;

namespace webapi.Models.ContextEF
{
    public class DataSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            string seedTTstr = $"[{{\"TaxYear\":2024,\"TaxPeriodDescription\":\"18% of taxable income\",\"TaxBracketStart\":1,\"TaxBracketEnd\":237100,\"TaxBasePercent\":0.18,\"TaxBaseAmount\":0.0}},{{\"TaxYear\":2024,\"TaxPeriodDescription\":\"42678 + 26% of taxable income above 237100\",\"TaxBracketStart\":237101,\"TaxBracketEnd\":370500,\"TaxBasePercent\":0.26,\"TaxBaseAmount\":42678.0}},{{\"TaxYear\":2024,\"TaxPeriodDescription\":\"77362 + 31% of taxable income above 370500\",\"TaxBracketStart\":370501,\"TaxBracketEnd\":512800,\"TaxBasePercent\":0.31,\"TaxBaseAmount\":77362.0}},{{\"TaxYear\":2024,\"TaxPeriodDescription\":\"121475 + 36% of taxable income above 512800\",\"TaxBracketStart\":512801,\"TaxBracketEnd\":673000,\"TaxBasePercent\":0.36,\"TaxBaseAmount\":121475.0}},{{\"TaxYear\":2024,\"TaxPeriodDescription\":\"179147 + 39% of taxable income above 673000\",\"TaxBracketStart\":673001,\"TaxBracketEnd\":857900,\"TaxBasePercent\":0.39,\"TaxBaseAmount\":179147.0}},{{\"TaxYear\":2024,\"TaxPeriodDescription\":\"251258 + 41% of taxable income above 857900\",\"TaxBracketStart\":857901,\"TaxBracketEnd\":1817000,\"TaxBasePercent\":0.41,\"TaxBaseAmount\":251258.0}},{{\"TaxYear\":2024,\"TaxPeriodDescription\":\"644489 + 45% of taxable income above 1817000\",\"TaxBracketStart\":1817001,\"TaxBracketEnd\":1817001,\"TaxBasePercent\":0.45,\"TaxBaseAmount\":644489.0}}]";
            string seedTRstr = $"[{{\"TaxYear\":2024,\"RebateType\":\"Primary\",\"RebateAmount\":17235.0,\"ThreshHoldAmount\":95750.0,\"RebateTypeDescription\":\"< 65\"}},{{\"TaxYear\":2024,\"RebateType\":\"Secondary\",\"RebateAmount\":9444.0,\"ThreshHoldAmount\":148217.0,\"RebateTypeDescription\":\"Between 65 and 75\"}},{{\"TaxYear\":2024,\"RebateType\":\"Tertiary\",\"RebateAmount\":3145.0,\"ThreshHoldAmount\":165689.0,\"RebateTypeDescription\":\"Older than 75\"}}]";

            var rebates = JsonConvert.DeserializeObject<List<TaxRebates>>(seedTRstr);
            var taxtables = JsonConvert.DeserializeObject<List<TaxTables>>(seedTTstr);
            try
            {
                using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetService<TaxablesDBContext>();

                    if (!context.TaxRebate.Any())
                    {
                        context.AddRange(rebates);
                        context.SaveChanges();
                    }

                    if (!context.TaxTable.Any())
                    {
                        context.AddRange(taxtables);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
    }
}
