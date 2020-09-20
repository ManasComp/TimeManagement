﻿using Xamarin.Forms;

 namespace TimeManagement.Behaviours
{
    public class NotNullBehaviour : Behavior<Entry>//changes the background color of entry block (login page)
    {
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += Bindable_TextChanged;
        }

        private void Bindable_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if(entry!=null)
                entry.BackgroundColor = entry.Text.Length > 0 ? Color.Default : Color.Red;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged += Bindable_TextChanged;
        }
    }
}