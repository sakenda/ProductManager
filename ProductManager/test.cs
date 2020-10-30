using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Herweck.DataObjects
{
    public class NotifyPropertyChanged
        : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected NotifyPropertyChanged()
        {
            this.ResetIsDirty();
        }

        //protected void onPropertyChanged(object sender, string inInfo)
        //{
        //    this.IsDirty = true;
        //    this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(inInfo));
        //}

        //[NotifyPropertyChangedInvocator]
        protected void onPropertyChanged(object sender, [CallerMemberName] string propertyName = "")
        {
            this.IsDirty = true;
            this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void ResetIsDirty()
        {
            this.IsDirty = false;
        }

        public bool IsDirty
        {
            get;
            private set;
        }
    }
}

namespace Herweck.DataObjects
{
    public class ListNotifyPropertyChanged<T>
        : BindingList<T>, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ListNotifyPropertyChanged()
            : base()
        {
        }

        protected void onPropertyChanged(object sender, string inInfo)
        {
            this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(inInfo));
        }

        public void AddItemEventhandler(T item)
        {
            NotifyPropertyChanged element;
            element = item as NotifyPropertyChanged;

            if (element != null)
            {
                element.PropertyChanged += Element_PropertyChanged;
            }
        }

        public new void Add(T item)
        {
            base.Add(item);
            this.AddItemEventhandler(item);
            this.onPropertyChanged(item, "Add");
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            this.AddItemEventhandler(item);
            this.onPropertyChanged(item, "Add");
        }

        private void Element_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.onPropertyChanged(sender, e.PropertyName);
        }

        public new void Remove(T item)
        {
            base.Remove(item);
            this.onPropertyChanged(item, "Remove");
        }
    }
}

namespace Projektdatenbank_Share.DataObjects
{
    public class UnifyHauptclusterListe
        : Herweck.DataObjects.NotifyPropertyChanged
    {
        public UnifyHauptclusterListe()
            : base()
        {
            this.UnifyHauptcluster = new Herweck.DataObjects.ListNotifyPropertyChanged<UnifyHauptcluster>();
            this.UnifyHauptcluster.AllowNew = true;
            this.UnifyHauptcluster.PropertyChanged += UnifyHauptcluster_PropertyChanged;
            this.UnifyHauptcluster2Del = new Herweck.DataObjects.ListNotifyPropertyChanged<UnifyHauptcluster>();
        }

        private void UnifyHauptcluster_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string propertyName;
            propertyName = "UnifyHauptcluster." + e.PropertyName;
            base.onPropertyChanged(sender, propertyName);
        }

        public Herweck.DataObjects.ListNotifyPropertyChanged<UnifyHauptcluster> UnifyHauptcluster { get; private set; }
        public Herweck.DataObjects.ListNotifyPropertyChanged<UnifyHauptcluster> UnifyHauptcluster2Del { get; private set; }
    }
}

namespace Projektdatenbank_Share.DataObjects
{
    public class UnifyHauptcluster
        : Herweck.DataObjects.NotifyPropertyChanged
    {
        private int myUnifyHauptclusterID;
        private string myBezeichnung;

        public UnifyHauptcluster()
           : base()
        {
            this.UnifyHauptclusterID = -1;

            this.Cluster = new Herweck.DataObjects.ListNotifyPropertyChanged<UnifyCluster>();
            this.Cluster2Del = new Herweck.DataObjects.ListNotifyPropertyChanged<UnifyCluster>();

            this.Cluster.AllowNew = true;
            this.Cluster.PropertyChanged += Cluster_PropertyChanged;
        }

        private void Cluster_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string propertyName;
            propertyName = "Cluster." + e.PropertyName;

            base.onPropertyChanged(sender, propertyName);
        }



        public int UnifyHauptclusterID
        {
            get
            {
                return this.myUnifyHauptclusterID;
            }
            set
            {
                this.myUnifyHauptclusterID = value;
                base.onPropertyChanged(this);
            }
        }

        public string Bezeichnung
        {
            get
            {
                return this.myBezeichnung;
            }
            set
            {
                this.myBezeichnung = value;
                base.onPropertyChanged(this);
            }
        }

        public Herweck.DataObjects.ListNotifyPropertyChanged<UnifyCluster> Cluster { get; private set; }
        public Herweck.DataObjects.ListNotifyPropertyChanged<UnifyCluster> Cluster2Del { get; private set; }
    }
}