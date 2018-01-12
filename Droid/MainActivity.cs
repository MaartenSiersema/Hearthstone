using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using ReactiveUI.AndroidSupport;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive;
using Android.Content;

namespace Heartstone.Droid
{
    [Activity(Label = "Heartstone", MainLauncher = true, Icon = "@mipmap/icon", Theme = "@style/Theme.Heartstone")]
    public class MainActivity : ReactiveAppCompatActivity<CardSetViewModel>
    {
        protected RecyclerView CardRecyclerView { get; set; }
        CardAdapter adapter;
        RecyclerView.LayoutManager layoutManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            this.WireUpControls();

            ViewModel = new CardSetViewModel();

            layoutManager = new LinearLayoutManager(this);
            CardRecyclerView.SetLayoutManager(layoutManager);

            adapter = new CardAdapter(ViewModel.CardCells);
            CardRecyclerView.SetAdapter(adapter);

            this.WhenActivated(d =>
            {
                this.WhenAnyObservable(view => view.adapter.ItemSelected)
                    .Subscribe(OnItemSelected)
                    .DisposeWith(d);

                Observable.Return(Unit.Default)
                          .InvokeCommand(ViewModel.RefreshCards)
                          .DisposeWith(d);
            });
        }

        protected override void OnResume()
        {
            base.OnResume();
        }

        void OnItemSelected(int position)
        {
            var CardViewModel = ViewModel.GetCardViewModel(position);

            var intent = CardDetailActivity.CreateIntent(this, CardViewModel.Card);
            var selectedItemViewHolder = layoutManager.FindViewByPosition(position);
            var sharedView = selectedItemViewHolder.FindViewById(Resource.Id.CardImageView);
            var transitionName = GetString(Resource.String.cardimage);
            var transitionActivityOptions = ActivityOptions.MakeSceneTransitionAnimation(this, sharedView, transitionName);

            StartActivity(intent, transitionActivityOptions.ToBundle());
        }
    }
}

