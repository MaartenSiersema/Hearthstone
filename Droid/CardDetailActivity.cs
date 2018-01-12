using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Views;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.AndroidSupport;

namespace Heartstone.Droid
{
    [Activity(Label = "CardDetailActivity", Theme = "@style/Theme.Heartstone")]
    public class CardDetailActivity : ReactiveAppCompatActivity<CardViewModel>
    {

        public const string CardExtra = "Card";

        TextView TitleTextView { get; set; }

        TextView CardTypeTextView { get; set; }

        TextView CardDescriptionTextView { get; set; }

        ImageViewAsync CardImageView { get; set; }

        Card _card;
        public Card Card
        {
            get
            {
                if (_card == null)
                {
                    var json = Intent.GetStringExtra(CardExtra);
                    if (json == null)
                        throw new ArgumentNullException(CardExtra, "You should pass a Card Extra");

                    _card = JsonConvert.DeserializeObject<Card>(json);
                }

                return _card;
            }
        }

        public static Intent CreateIntent(Context context, Card card)
        {
            var intent = new Intent(context, typeof(CardDetailActivity));
            intent.PutExtra(CardExtra, JsonConvert.SerializeObject(card));
            return intent;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = new CardViewModel(Card);
            SetContentView(Resource.Layout.CardDetailActivity);

            this.WireUpControls();

            this.WhenActivated(d =>
            {
                this.Bind(ViewModel, vm => vm.Card.Name, view => view.TitleTextView.Text)
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.Card.Type, view => view.CardTypeTextView.Text)
                    .DisposeWith(d);

                this.Bind(ViewModel, vm => vm.Card.Text, view => view.CardDescriptionTextView.Text)
                    .DisposeWith(d);

                this.WhenAnyValue(vh => vh.ViewModel.Card.Img)
                    .Subscribe(imageUrl => ImageService.Instance.LoadUrl(imageUrl)
                               .Retry(3, 100)
                               .Into(CardImageView));
            });
        }
    }
}
