using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace PiskyBrisky
{
   public sealed partial class Usernames : Page
    {
        public const int MIN_LENGTH_NOM = 2;
        public const int MAX_LENGTH_NOM = 20;

        List <string> usernames = new List<string>();
        private int idx_jugador = 0;
        public Usernames()
        {
            this.InitializeComponent();
            MostrarEntradaJugador();
        }

        //S'executa al introduir un nom
        private void Introduir_Click(object sender, RoutedEventArgs e)
        {
            string nom = txbNom.Text.Trim();

            if (nom.Length < MIN_LENGTH_NOM || nom.Length > MAX_LENGTH_NOM)
            {
                txbError.Text = $"El nom ha de tenir entre {MIN_LENGTH_NOM} y {MAX_LENGTH_NOM} caràcters.";
                txbError.Visibility = Visibility.Visible;
            }
            else if (VerificarNomRepetit(nom))
            {
                txbError.Text = "Aquest nom ja està en ús. Introdueix un altre.";
                txbError.Visibility = Visibility.Visible;
            }
            else
            {
                //Nom vàlid
                usernames.Add(nom);
                idx_jugador++;

                if (idx_jugador < 4)
                {
                    MostrarEntradaJugador();
                }
                else
                {
                    //Jugadors afegits, mostrem el MainPage. També pasem la llista dels usuaris per a manipularlos al MainPage.
                    Frame.Navigate(typeof(MainPage), usernames);
                }
            }
        }

        //Verifica si existeix el nom a la llista "usernames"
        private bool VerificarNomRepetit(string nom)
        {
            return usernames.Contains(nom);
        }

        //Mostra el TextBlock per introduir el nom del jugador
        private void MostrarEntradaJugador()
        {
            txbTitol.Text = $"Introdueix el nom del Jugador {idx_jugador + 1}:";
            txbNom.Text = string.Empty;
            txbError.Visibility = Visibility.Collapsed;
        }

        //Executa el mateix codi al presionar enter que al fer click al botó d'introduïr
        private void txtNom_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Introduir_Click(sender, e);
            }
        }
    }
}
