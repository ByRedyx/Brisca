using System;

namespace PiskyBrisky.Model
{
    public class Carta
    {
        private int numero;
        private Pals pal;

        public Carta(int numero, Pals pal)
        {
            Numero = numero;
            Pal = pal;
        }

        public String UrlCarta => "ms-appx:///Cards/" + pal.ToString().ToLower().Substring(0, 1) + numero + ".png";
        public int Numero { get => numero; set => numero = value; }
        public Pals Pal { get => pal; set => pal = value; }
    }
}