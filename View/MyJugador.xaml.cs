using PiskyBrisky.Model;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace PiskyBrisky.View
{
    public sealed partial class MyJugador : UserControl
    {
        private bool guanyador = false;
        private Carta cartaTapped = null;

        public MyJugador()
        {
            this.InitializeComponent();
        }

        public Jugador ElJugador
        {
            get { return (Jugador)GetValue(ElJugadorProperty); }
            set { SetValue(ElJugadorProperty, value); }
        }

        public static readonly DependencyProperty ElJugadorProperty =
            DependencyProperty.Register("ElJugador", typeof(Jugador), typeof(MyJugador), new PropertyMetadata(null));

        private void Carta_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Image cartaImage && sender != null)
            {
                Jugador jugadorActual = ElJugador;
                int idCartaTapped = int.Parse(cartaImage.Tag.ToString());

                /*Abans utilitzava la següent linia per mostrar la carta, pero...
                només funcionava la primera ronda, ja que a partir de la segona ronda
                si jugaba una carta que havia estat a la mateixa posició d'una carta
                previament jugada, mostrava la mateixa carta jugada previament. Per
                alguna raó, l'objecte "sender" sempre envia la mateixa carta*/
                //Carta carta = (Carta)cartaImage.DataContext;
                switch (idCartaTapped)
                {
                    case 1:
                        cartaTapped = ElJugador.Carta1;
                        break;
                    case 2:
                        cartaTapped = ElJugador.Carta2;
                        break;
                    case 3:
                        cartaTapped = ElJugador.Carta3;
                        break;
                }
                
                //Només permet fer modificacions si es el seu torn
                if (jugadorActual.EsElMeuTorn)
                {
                    imgCartaJugador1.Opacity = 1;
                    imgCartaJugador2.Opacity = 1;
                    imgCartaJugador3.Opacity = 1;
                    if (jugadorActual.CartaSeleccionada != cartaTapped)
                    {
                        jugadorActual.CartaSeleccionada = cartaTapped;
                        cartaImage.Opacity = 0.7; //Efecte visual per indicar que la carta està seleccionada
                    }
                    else
                    {
                        //Deseleccionem la carta
                        jugadorActual.CartaSeleccionada = null;
                    }

                    actualitzarBotoJugar();
                }
            }
        }

        //Activa o desactiva el botó per jugar carta
        private void actualitzarBotoJugar()
        {
            //Verifica si alguna carta està seleccionada i si es el torn del jugador
            bool algunaCartaSeleccionada = ElJugador.CartaSeleccionada != null && ElJugador.EsElMeuTorn;
            btnJugar.IsEnabled = algunaCartaSeleccionada;
        }

        public void mostrarGuanyador()
        {
            guanyador = true;
        }

        //Actualitza la part gràfica segons el torn del jugador o si guanya la ronda
        public void actualitzarView()
        {
            //Actualitza els punts al TextBox
            txbPunts.Text = ElJugador.Punts.ToString();
            if (ElJugador.EsElMeuTorn)
            {
                //Canvia el fons a Vermell si es el seu torn o a Verd si ha guanyat la ronda anterior
                if (guanyador)
                {
                    gridJugador.Background = new SolidColorBrush(Colors.LimeGreen);
                } else
                {
                    gridJugador.Background = new SolidColorBrush(Color.FromArgb(255, 255, 54, 50));
                }
                guanyador = false;

                //Mostra les seves cartes
                if (ElJugador.Carta1 != null)
                {
                    imgCartaJugador1.Visibility = Visibility.Visible; 
                    imgCartaJugador1.Opacity = 1;
                    imgCartaJugador1.Source = new BitmapImage(new Uri(ElJugador.Carta1.UrlCarta));
                }
                if (ElJugador.Carta2 != null)
                {
                    imgCartaJugador2.Visibility = Visibility.Visible;
                    imgCartaJugador2.Opacity = 1;
                    imgCartaJugador2.Source = new BitmapImage(new Uri(ElJugador.Carta2.UrlCarta));
                }
                if (ElJugador.Carta3 != null)
                {
                    imgCartaJugador3.Visibility = Visibility.Visible;
                    imgCartaJugador3.Opacity = 1;
                    imgCartaJugador3.Source = new BitmapImage(new Uri(ElJugador.Carta3.UrlCarta));
                }

                //Mostra el botó
                btnJugar.Visibility = Visibility.Visible;
            }
            else
            {
                //Canvia el fons a LightSkyBlue
                gridJugador.Background = new SolidColorBrush(Colors.LightSkyBlue);

                //Mostra back.png
                if (ElJugador.Carta1 != null)
                {
                    imgCartaJugador1.Visibility = Visibility.Visible;
                    imgCartaJugador1.Opacity = 1;
                    imgCartaJugador1.Source = new BitmapImage(new Uri("ms-appx:///Cards/back.png"));
                }
                if (ElJugador.Carta2 != null)
                {
                    imgCartaJugador2.Visibility = Visibility.Visible;
                    imgCartaJugador2.Opacity = 1;
                    imgCartaJugador2.Source = new BitmapImage(new Uri("ms-appx:///Cards/back.png"));
                }
                if (ElJugador.Carta3 != null)
                {
                    imgCartaJugador3.Visibility = Visibility.Visible;
                    imgCartaJugador3.Opacity = 1;
                    imgCartaJugador3.Source = new BitmapImage(new Uri("ms-appx:///Cards/back.png"));
                }              

                //Oculta el botó
                btnJugar.Visibility = Visibility.Collapsed;
            }
            btnJugar.IsEnabled = false;
        }

        //S'executa al jugar una carta
        private void btnJugar_Click(object sender, RoutedEventArgs e)
        {
            //Oculta la carta jugada
            if (ElJugador.CartaSeleccionada == ElJugador.Carta1)
            {
                imgCartaJugador1.Visibility = Visibility.Collapsed;
                ElJugador.Carta1 = null;
            }
            else if (ElJugador.CartaSeleccionada == ElJugador.Carta2)
            {
                imgCartaJugador2.Visibility = Visibility.Collapsed;
                ElJugador.Carta2 = null;
            }
            else if (ElJugador.CartaSeleccionada == ElJugador.Carta3)
            {
                imgCartaJugador3.Visibility = Visibility.Collapsed;
                ElJugador.Carta3 = null;
            }
            
            //Actualitza les imatges jugades, les col·leccions de cartes jugades i torns de jugadors
            if (Window.Current.Content is Frame frame && frame.Content is MainPage mainPage) //No se ni que significa pero funciona :)
            {
                mainPage.jugarCarta(ElJugador);
            }
        }
    }
}