namespace FelicaCashingSystemV2.Views
{
    class ReadonlyTextFileViewModel : MetroWindowViewModelBase
    {
        private string content = string.Empty;
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
    }
}
