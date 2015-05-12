using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Markout.Output.Inlines.Controls {

    public class InlinesTextBlock : TextBlock {

        public static readonly DependencyProperty TextInlinesProperty = DependencyProperty.Register(
            "TextInlines",
            typeof(ObservableCollection<Inline>),
            typeof(InlinesTextBlock),
            new PropertyMetadata(
                new ObservableCollection<Inline>(),
                (s, e) => {
                    InlinesTextBlock itb = s as InlinesTextBlock;
                    if (itb != null) {
                        itb.WatchInlines((ObservableCollection<Inline>)e.NewValue);
                    }
                }
                ));

        public ObservableCollection<Inline> TextInlines {
            get { return (ObservableCollection<Inline>)GetValue(TextInlinesProperty); }
            set { SetValue(TextInlinesProperty, value); }
        }

        private void WatchInlines(ObservableCollection<Inline> inlineCollection) {
            if (TextInlines != null) {
                TextInlines.CollectionChanged -= TextInlinesOnCollectionChanged;
            }
            if (inlineCollection != null) {
                inlineCollection.CollectionChanged += TextInlinesOnCollectionChanged;
            }
        }

        private void TextInlinesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs) {
            Inlines.Clear();
            ObservableCollection<Inline> inlinesCollection = sender as ObservableCollection<Inline>;
            if (inlinesCollection != null) {
                foreach (Inline inline in inlinesCollection) {
                    Inlines.Add(inline);
                }
            }
        }
    }
}
