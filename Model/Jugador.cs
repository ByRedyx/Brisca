namespace PiskyBrisky.Model
{
    public class Jugador
    {
        private string nom;
        private bool esElMeuTorn;
        private Carta carta1;
        private Carta carta2;
        private Carta carta3;
        private Carta cartaSeleccionada;
        private int punts;
        
        public Jugador(string nom, int id)
        {
            Nom = "P" + id + ": " + nom;
            EsElMeuTorn = false;
            Carta1 = null;
            Carta2 = null;
            Carta3 = null;
            CartaSeleccionada = null;
            Punts = 0;
        }

        public string Nom { get => nom; set => nom = value; }
        public bool EsElMeuTorn { get => esElMeuTorn; set => esElMeuTorn = value; }
        public Carta Carta1 { get => carta1; set => carta1 = value; }
        public Carta Carta2 { get => carta2; set => carta2 = value; }
        public Carta Carta3 { get => carta3; set => carta3 = value; }
        public Carta CartaSeleccionada { get => cartaSeleccionada; set => cartaSeleccionada = value; }
        public int Punts { get => punts; set => punts = value; }
    }
}
