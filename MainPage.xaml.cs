using PiskyBrisky.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace PiskyBrisky
{
    public sealed partial class MainPage : Page
    {
        public const int MAX_NUM = 10;
        public const int MAX_PAL = 4;
        public const int MAX_CARTES = MAX_NUM * MAX_PAL;
        public const int MAX_JUGADORS = 4;
        private Jugador[] jugadors = new Jugador[MAX_JUGADORS];
        private string[] usernames = new string[MAX_JUGADORS];
        private List<Carta> baralla = new List<Carta>();
        private List<Carta> cartesJugades = new List<Carta>();
        private Carta brisca;
        private int idx_jugador;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            executarProgramaBrisca();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Reb el paràmetre dels noms dels usuaris
            if (e.Parameter is List<string> llistaUsernames)
            {
                int i = 0;
                foreach (string username in llistaUsernames)
                {
                    //Crea un nou jugador amb el nom d'usuari
                    usernames[i] = username;
                    i++;
                }
            }
        }

        /*Creo un métode per a executar tot el "main" cada vegada
        que acaba una partida i es torna a jugar una nova*/
        private void executarProgramaBrisca()
        {
            //Creo els jugadors amb els noms introduits
            for (int i = 0; i < MAX_JUGADORS; i++)
            {
                jugadors[i] = new Jugador(usernames[i], i + 1);
            }

            //Reinicio dades per si es la segona vegada que es juga
            baralla.Clear();
            cartesJugades.Clear();

            txbWinner.Visibility = Visibility.Collapsed;
            btnNewGame.Visibility = Visibility.Collapsed;
            imgBrisca.Visibility = Visibility.Visible;
            imgPila.Visibility = Visibility.Visible;

            assignarTornRandom();
            inicialitzarCartes();
            repartirCartes();

            view1.ElJugador = jugadors[0];
            view2.ElJugador = jugadors[1];
            view3.ElJugador = jugadors[2];
            view4.ElJugador = jugadors[3];

            actualitzarView();
        }

        //Randomitza qui comença la partida
        private void assignarTornRandom()
        {
            Random rand = new Random();
            idx_jugador = rand.Next(0, 4);
            jugadors[idx_jugador].EsElMeuTorn = true;
        }

        //Crea totes les cartes que es juguen i les barreja
        private void inicialitzarCartes()
        {
            Random rand = new Random();

            foreach (Pals pal in Enum.GetValues(typeof(Pals)))
            {
                for (int numero = 1; numero <= 12; numero++)
                {
                    if (numero != 8 && numero != 9) {
                        baralla.Add(new Carta(numero, pal));
                    }
                }
            }

            int qt = baralla.Count;

            //Algoritme de "Fisher-Yates"
            for (int i = qt - 1; i > 0; i--)
            {
                int j = rand.Next(0, i + 1);
                Carta temp = baralla[i];
                baralla[i] = baralla[j];
                baralla[j] = temp;
            }
            brisca = baralla.Last();
            imgBrisca.Source = new BitmapImage(new Uri(brisca.UrlCarta));
        }

        //Reparteix cartes als jugadors
        private void repartirCartes()
        {
            for (int i = 0; i < MAX_JUGADORS; i++)
            {
                //Revisa quines cartes falten al jugador
                if (jugadors[i].Carta1 == null && baralla.Count > 0)
                {
                    jugadors[i].Carta1 = baralla[0];
                    baralla.RemoveAt(0);
                }
                if (jugadors[i].Carta2 == null && baralla.Count > 0)
                {
                    jugadors[i].Carta2 = baralla[0];
                    baralla.RemoveAt(0);
                }
                if (jugadors[i].Carta3 == null && baralla.Count > 0)
                {
                    jugadors[i].Carta3 = baralla[0];
                    baralla.RemoveAt(0);
                }

                /*Quan queda una carta, només es mostrarà la brisca al centre
                i deixarà de ser visible la pila de cartes*/
                if (baralla.Count == 1)
                {
                    imgPila.Visibility = Visibility.Collapsed;
                }
                //Quan no queden cartes, no es mostrarà res al centre
                else if (baralla.Count == 0)
                {
                    imgBrisca.Visibility = Visibility.Collapsed;
                    break;
                }
            }
        }

        //Actualitza la part gràfica segons el torn del jugador
        private void actualitzarView()
        {
            view1.actualitzarView();
            view2.actualitzarView();
            view3.actualitzarView();
            view4.actualitzarView();
        }

        //S'executa al jugar una carta des de "MyJugador.xaml.cs"
        public void jugarCarta(Jugador elJugador)
        {
            //Deixem les cartes visibles al acabar una ronda i al jugar una carta les amaguem totes
            if (cartesJugades.Count == 0)
            {
                imgCarta1.Visibility = Visibility.Collapsed;
                imgCarta2.Visibility = Visibility.Collapsed;
                imgCarta3.Visibility = Visibility.Collapsed;
                imgCarta4.Visibility = Visibility.Collapsed;
                imgCarta1.Opacity = 1;
                imgCarta2.Opacity = 1;
                imgCarta3.Opacity = 1;
                imgCarta4.Opacity = 1;
                spCarta1.Background = null;
                spCarta2.Background = null;
                spCarta3.Background = null;
                spCarta4.Background = null;
            }
            cartesJugades.Add(elJugador.CartaSeleccionada);

            switch (cartesJugades.Count)
            {
                case 1:
                    imgCarta1.Visibility = Visibility.Visible;
                    imgCarta1.Source = new BitmapImage(new Uri(elJugador.CartaSeleccionada.UrlCarta));
                    break;
                case 2:
                    imgCarta2.Visibility = Visibility.Visible;
                    imgCarta2.Source = new BitmapImage(new Uri(elJugador.CartaSeleccionada.UrlCarta));
                    break;
                case 3:
                    imgCarta3.Visibility = Visibility.Visible;
                    imgCarta3.Source = new BitmapImage(new Uri(elJugador.CartaSeleccionada.UrlCarta));
                    break;
                case 4:
                    imgCarta4.Visibility = Visibility.Visible;
                    imgCarta4.Source = new BitmapImage(new Uri(elJugador.CartaSeleccionada.UrlCarta));
                    break;
            }

            //Deixa de ser el seu torn després de jugar
            jugadors[idx_jugador].EsElMeuTorn = false;

            //Si es l'última carta jugada de la ronda, busquem guanyador i repartim cartes
            if (cartesJugades.Count == MAX_JUGADORS)
            {
                buscarGuanyador();
                cartesJugades.Clear();
                repartirCartes();
            } else
            //Si no es l'últim, passem al següent jugador
            {
                if (idx_jugador < 3)
                {
                    idx_jugador++;
                }
                else
                {
                    idx_jugador = 0;
                }
            }
            //Jugador que ha guanyat la ronda o següent jugador
            jugadors[idx_jugador].EsElMeuTorn = true;
            actualitzarView();
        }

        //Busca el guanyador de la ronda jugada
        private void buscarGuanyador()
        {
            Carta cartaGuanyadora = null, cartaJugadorBrisca = null, cartaJugadorSortida = null;
            int valorGuanyador = 0, valorPalBrisca = 0, valorPalSortida = 0;
            int indexJugadorGuanyador = -1, valorCartaActual = 0;

            //Busquem entre les 4 cartes, la guanyadora
            for (int i = 0; i < cartesJugades.Count; i++)
            {
                Carta cartaActual = cartesJugades[i];

                //Segons el numero de la carta, agafem el seu valor
                switch (cartaActual.Numero)
                {
                    case 1:
                        valorCartaActual = 11;
                        break;
                    case 3:
                        valorCartaActual = 10;
                        break;
                    case 12:
                        valorCartaActual = 4;
                        break;
                    case 11:
                        valorCartaActual = 3;
                        break;
                    case 10:
                        valorCartaActual = 2;
                        break;
                    default:
                        valorCartaActual = 0;
                        break;
                }

                if (cartaActual.Pal == brisca.Pal && valorCartaActual > valorPalBrisca)
                {
                    valorPalBrisca = valorCartaActual;
                    cartaJugadorBrisca = cartaActual;
                } else if (cartaActual.Pal == cartesJugades[0].Pal && valorCartaActual > valorPalSortida)
                {
                    valorPalSortida = valorCartaActual;
                    cartaJugadorSortida = cartaActual;
                }
            }

            if (cartaJugadorBrisca != null) //Guanya el pal de la brisca
            {
                cartaGuanyadora = cartaJugadorBrisca;
                valorGuanyador = valorPalBrisca;
            }
            else if (cartaJugadorSortida != null) //No s'ha jugat ningun pal igual al de la brisca, guanya el pal de sortida
            {
                cartaGuanyadora = cartaJugadorSortida;
                valorGuanyador = valorPalSortida;
            }

            //Busquem i cambiem el color de la carta guanyadora
            for (int i = 0; i < MAX_JUGADORS; i++)
            {
                if (jugadors[i].CartaSeleccionada == cartaGuanyadora)
                {
                    if (jugadors[i].CartaSeleccionada.Numero == cartesJugades[0].Numero && jugadors[i].CartaSeleccionada.Pal == cartesJugades[0].Pal)
                    {
                        imgCarta1.Opacity = 0.7;
                        spCarta1.Background = new SolidColorBrush(Colors.LimeGreen);
                    } else if (jugadors[i].CartaSeleccionada.Numero == cartesJugades[1].Numero && jugadors[i].CartaSeleccionada.Pal == cartesJugades[1].Pal)
                    {
                        imgCarta2.Opacity = 0.7;
                        spCarta2.Background = new SolidColorBrush(Colors.LimeGreen);
                    }
                    else if (jugadors[i].CartaSeleccionada.Numero == cartesJugades[2].Numero && jugadors[i].CartaSeleccionada.Pal == cartesJugades[2].Pal)
                    {
                        imgCarta3.Opacity = 0.7;
                        spCarta3.Background = new SolidColorBrush(Colors.LimeGreen);
                    }
                    else if (jugadors[i].CartaSeleccionada.Numero == cartesJugades[3].Numero && jugadors[i].CartaSeleccionada.Pal == cartesJugades[3].Pal)
                    {
                        imgCarta4.Opacity = 0.7;
                        spCarta4.Background = new SolidColorBrush(Colors.LimeGreen);
                    }

                    //Assignem puntuació
                    jugadors[i].CartaSeleccionada = null;
                    jugadors[i].Punts += valorGuanyador;
                    indexJugadorGuanyador = i;
                    break;
                }
            }

            //Mostrem qui ha guanyat
            if (indexJugadorGuanyador == 0)
            {
                view1.mostrarGuanyador();
            } else if (indexJugadorGuanyador == 1)
            {
                view2.mostrarGuanyador();
            } else if (indexJugadorGuanyador == 2)
            {
                view3.mostrarGuanyador();
            } else if (indexJugadorGuanyador == 3)
            {
                view4.mostrarGuanyador();
            }

            /*Si indexJugadorGuanyador == -1 significa que no hi ha guanyador
            (no s'ha jugat carta del mateix pal que la brisca ni cartes amb valor
            del mateix pal que la carta de sortida)*/
            if (indexJugadorGuanyador == -1)
            {
                if (idx_jugador < 3)
                {
                    idx_jugador++;
                }
                else
                {
                    idx_jugador = 0;
                }
            } else
            {
                idx_jugador = indexJugadorGuanyador;
            }

            //Si no tenen mes cartes els jugadors, busquem guanyador de la partida
            if (jugadors[0].Carta1 == null && jugadors[0].Carta2 == null && jugadors[0].Carta3 == null &&
                jugadors[1].Carta1 == null && jugadors[1].Carta2 == null && jugadors[1].Carta3 == null &&
                jugadors[2].Carta1 == null && jugadors[2].Carta2 == null && jugadors[2].Carta3 == null &&
                jugadors[3].Carta1 == null && jugadors[3].Carta2 == null && jugadors[3].Carta3 == null)
            {
                //Utilitzo una llista per si hi ha un empat
                int puntsGuanyador = -1;

                //Busco el màxim de punts aconseguits
                for (int i = 0; i < MAX_JUGADORS; i++)
                {
                    if (jugadors[i].Punts > puntsGuanyador)
                    {
                        puntsGuanyador = jugadors[i].Punts;
                    }
                }

                /*Busco els jugadors que tenen el màxim de punts aconseguits a la partida
                i els mostro com guanyadors*/
                List<Jugador> guanyadors = new List<Jugador>();
                for (int i = 0; i < MAX_JUGADORS; i++)
                {
                    if (jugadors[i].Punts == puntsGuanyador)
                    {
                        guanyadors.Add(jugadors[i]);
                    }
                }
                String txtGuanyador;
                if (guanyadors.Count > 1)
                {
                    txtGuanyador = "WINNERS: ";
                }
                else
                {
                    txtGuanyador = "WINNER: ";
                }

                for (int i = 0; i < guanyadors.Count; i++)
                {
                    txtGuanyador += guanyadors[i].Nom.Substring(4);
                    if (guanyadors.Count > i + 1)
                    {
                        txtGuanyador += ", ";
                    }
                    else
                    {
                        txtGuanyador += ".";
                    }
                }

                txbWinner.Text = txtGuanyador;
                txbWinner.Visibility = Visibility.Visible;
                btnNewGame.Visibility = Visibility.Visible;
                imgCarta1.Visibility = Visibility.Collapsed;
                imgCarta2.Visibility = Visibility.Collapsed;
                imgCarta3.Visibility = Visibility.Collapsed;
                imgCarta4.Visibility = Visibility.Collapsed;
            }
        }

        //Crea una nova partida
        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            executarProgramaBrisca();
        }
    }
}