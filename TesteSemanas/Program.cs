using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TesteSemanas
{
    class Program
    {
        static void Main(string[] args)
        {

            List<DateTime> datas = new();
            datas.Add(new DateTime(2021, 02, 06));
            datas.Add(new DateTime(2021, 06, 03));
            datas.Add(new DateTime(2021, 06, 17));
            datas.Add(new DateTime(2021, 10, 01));
            datas.Add(new DateTime(2021, 11, 01));
            datas.Add(new DateTime(2021, 12, 20));

            datas.Sort();

            List<Semana> semanas = new();

            for (int i = 0; i < datas.Count; i += 2)
            {
                semanas.AddRange(ListaSemanasDeUmPeriodo(datas[i], datas[i + 1], DayOfWeek.Monday));
            }

            DateTime dataReferencia = new(2021, 12, 04); //data de referencia, geralmente é a do dia corrente

            Semana semana = semanas.FirstOrDefault(x => x.InicioSemana <= dataReferencia &&  dataReferencia <= x.FimSemana); //pega qual semana aquela data se encontra

            int numeroSemana = -1;

            if (semana != null)
                numeroSemana = semanas.IndexOf(semana) + 1; //verifica o numero da semana recuperada

            Console.WriteLine("Numero semana: {0}", numeroSemana);
        }



        public class Semana
        {
            public DateTime InicioSemana { get; set; }
            public DateTime FimSemana { get; set; }
        }

        public static Semana GetPrimeiroDiaSemana()
        {
            DayOfWeek x = DateTime.Today.DayOfWeek;

            DateTime startOfWeek = DateTime.Today.AddDays(-1 * (int)x);
            DateTime endOfWeek = DateTime.Today.AddDays(6 - (int)x);

            return new Semana { InicioSemana = startOfWeek, FimSemana = endOfWeek };
        }

        //1- Data de Inicio preenchida entre segunda à sexta inicia sempre na segunda-feira da semana corrente
        //2- Data preenchida entre sábado e domingo inicio sempre na próxima segunda.
        //3- Visualização semana: As semanas devem aparecer sempre com inicio em uma segunda e término no domingo
        //4- Semanas que tenham atrito  de data de período letivo e férias devem ser visualizadas em períodos mais "curto" correspondendo a visualização somente até data limite de inicio de férias. O inicio das mesmas deverá ocorrer no dia indicado como primeiro dia de inicio de férias.
        public static List<Semana> ListaSemanasDeUmPeriodo(DateTime inicioPeriodo, DateTime fimPeriodo, DayOfWeek diaInicioSemana)
        {
            const int daysInWeek = 7;
            List<Semana> result = new();

            var daysToAdd = ((int)diaInicioSemana - (int)inicioPeriodo.DayOfWeek);
            if (inicioPeriodo.DayOfWeek == DayOfWeek.Saturday)
            {
                daysToAdd += daysInWeek;
            }

            DateTime dataFim = fimPeriodo.AddDays((int)DayOfWeek.Friday - (int)fimPeriodo.DayOfWeek); //pega a ultima sexta feira do período como data limite pra se iniciar uma semana

            for (DateTime dataInicioSemana = inicioPeriodo.AddDays(daysToAdd); dataInicioSemana < dataFim; dataInicioSemana = dataInicioSemana.AddDays(daysInWeek))
            {
                DateTime dataFimSemana = dataInicioSemana.AddDays(6);
                result.Add(new Semana { InicioSemana = dataInicioSemana < inicioPeriodo ? inicioPeriodo : dataInicioSemana, FimSemana = dataFimSemana > fimPeriodo ? fimPeriodo : dataFimSemana });

            }

            return result;
        }

    }
}
