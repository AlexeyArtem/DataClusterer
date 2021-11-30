using Accord.Statistics.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClusterer
{
    static class DataConverter
    {

        public static IList<double[]> ReduceDemension(IList<double[]> data, int demension)
        {
            if (demension <= 0) throw new ArgumentException("Размерность не может быть отрицательным числом или нулем");

            PrincipalComponentAnalysis pca = new PrincipalComponentAnalysis()
            {
                Method = PrincipalComponentMethod.Center,
                Whiten = false, //Нужно ли номармализовывать выходные данные
                NumberOfOutputs = demension //Размерность выходных векторов, которые должны быть сгенерированы моделью
            };
            var arrData = data.ToArray();
            
            //Изучение модели данных
            pca.Learn(arrData);

            //Преобразование входных данных модели
            double[][] output = pca.Transform(arrData);

            return output;
        }

        public static IList<double[]> Normalize(IList<double[]> data)
        {
            throw new Exception();
        }
    }
}
