using System;
using Android.Support.V7.Widget;
using Android.Views;
using ReactiveUI.Android.Support;
using System.Collections.Generic;
using ReactiveUI;
using System.Reactive.Subjects;
using Android.Widget;
using System.Reactive.Linq;
using Android.Text;
using FFImageLoading;
using FFImageLoading.Views;
using Android.OS;
using System.Reactive.Threading.Tasks;
using System.Reactive.Disposables;

namespace Heartstone.Droid
{
    public class CardAdapter : ReactiveRecyclerViewAdapter<CardViewModel>
    {
        public Subject<int> ItemSelected;

        public CardAdapter(IReadOnlyReactiveList<CardViewModel> collection) : base(collection)
        {
            ItemSelected = new Subject<int>();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.CardListItem, parent, false);
            var holder = new CardViewHolder(view);
            holder.Selected.Subscribe(ItemSelected);
            return holder;
        }


        public class CardViewHolder : ReactiveRecyclerViewViewHolder<CardViewModel>
        {
            protected ImageViewAsync CardImageView { get; set; }
            protected TextView TitleTextView { get; set; }
            protected TextView DescriptionTextView { get; set; }

            public CardViewHolder(View itemView) : base(itemView)
            {
                this.WireUpControls();

                this.OneWayBind(ViewModel, vm => vm.Card.Name, vh => vh.TitleTextView.Text);

                this.OneWayBind(ViewModel, vm => vm.DescriptionLabel, vh => vh.DescriptionTextView.TextFormatted, text => Html.FromHtml(text.Replace("\\n", "<br>")));

                this.WhenAnyValue(vh => vh.ViewModel.Card.Img)
                    .SelectMany(imageUrl => ImageService.Instance.LoadUrl(imageUrl)
                               .DownSampleInDip(75, 75)
                               .Retry(3, 100)
                               .IntoAsync(CardImageView)
                                .ToObservable())
                    .Subscribe();
            }
        }
    }
}
