using System;
using ReactiveUI;
using System.Reactive;
using Splat;
using System.Reactive.Linq;
using System.Linq;
using System.Collections.Generic;

namespace Heartstone
{
    public class CardSetViewModel : ReactiveObject
    {
        public readonly ReactiveList<CardViewModel> CardCells;

        CardList _cardList;
        public CardList CardList
        {
            get
            {
                return _cardList;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _cardList, value);
            }
        }

        public readonly ReactiveCommand<Unit, CardList> RefreshCards;

        public CardSetViewModel()
        {
            CardCells = new ReactiveList<CardViewModel>();
            RefreshCards = ReactiveCommand.CreateFromObservable(() => HeartstoneRestService.Instance.GetCardSet("Basic"), null, RxApp.MainThreadScheduler);

            RefreshCards.ThrownExceptions.Subscribe(HandleRefreshCardsException);
            RefreshCards.BindTo(this, vm => vm.CardList);

            this.WhenAnyValue(vm => vm.CardList).Where(cards => cards != null).Subscribe(UpdateCells);
        }

        void HandleRefreshCardsException(Exception e)
        {
            this.Log().WarnException("Error while refeshing cards", e);
        }

        public CardViewModel GetCardViewModel(int selectedCard)
        {
            return CardCells[selectedCard];
        }

        void UpdateCells(CardList cardList)
        {
            //var rand = new Random();
            //cardList = new CardList(cardList.OrderBy(card => rand.Next()));

            var newCells = cardList.OrderBy(card => card.Name)
                                   .Select(card => new CardViewModel(card));

            if (!CardCells.SequenceEqual(newCells, new CardCellComparer()))
            {
                CardCells.Clear();
                CardCells.AddRange(newCells);
            }
        }

        public class CardCellComparer : IEqualityComparer<CardViewModel>
        {
            public bool Equals(CardViewModel x, CardViewModel y)
            {
                return x?.Card.CardId == y?.Card.CardId;
            }

            public int GetHashCode(CardViewModel obj)
            {
                return Int32.Parse(obj.Card.DbfId);
            }
        }
    }
}

