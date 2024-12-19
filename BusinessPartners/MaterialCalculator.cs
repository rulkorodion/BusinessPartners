using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessPartners
{
    public static class MaterialCalculator
    {
        public static int CalculateMaterialRequirement(int productTypeId, int materialTypeId, int quantity, double param1, double param2)
        {
            if (productTypeId <= 0 || materialTypeId <= 0 || quantity <= 0 || param1 <= 0 || param2 <= 0)
                return -1;

            double coefficient = GetProductCoefficient(productTypeId);
            double defectRate = GetMaterialDefectRate(materialTypeId);

            if (coefficient <= 0 || defectRate < 0)
                return -1;

            double materialPerUnit = param1 * param2 * coefficient;
            double totalMaterial = materialPerUnit * quantity;
            totalMaterial += totalMaterial * defectRate;
            return (int)Math.Ceiling(totalMaterial);
        }

        private static double GetProductCoefficient(int productTypeId)
        {
            using (var context = new Entityes())
            {
                var productType = context.ProductType.FirstOrDefault(pt => pt.ProductTypeID == productTypeId);
                return productType != null ? (double)productType.Coefficient : -1;
            }
        }


        private static double GetMaterialDefectRate(int materialTypeId)
        {
            using (var context = new Entityes())
            {
                var materialType = context.MaterialType.FirstOrDefault(mt => mt.MaterialTypeID == materialTypeId);
                return materialType != null ? (double)materialType.DefectRate : -1;
            }
        }
    }
}
