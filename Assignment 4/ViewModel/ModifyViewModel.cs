using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight; //For mvvmlight
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;//for class Messenger
using System.Windows.Input;
using System.Collections.ObjectModel;
using Assignment_4.Messages;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Windows;

namespace Assignment_4.ViewModel
{
    public class ModifyViewModel : ViewModelBase
    {
        public RelayCommand<IClosable> AcceptCommand { get; private set; }
        public RelayCommand<IClosable> CancelCommand { get; private set; }

        private int custID;
        private string modname;
        private string modaddress;
        private string modcity;
        private string modzipcode;
        private string modstate;
        private Customer selectedCustomer = new Customer();

        private ObservableCollection<State> states = new ObservableCollection<State>();
        private State selectedstate = new State();

        public ModifyViewModel()
        {
            AcceptCommand = new RelayCommand<IClosable>(this.AcceptMethod);
            CancelCommand = new RelayCommand<IClosable>(this.CancelMethod);

            var stateslist =
                (from state in MMABooksClass.context.States orderby state.StateName select state).ToList();
            states = new ObservableCollection<State>(stateslist);

            Messenger.Default.Register<MessageMember>(this, RecieveCustomerID);

        }

        public void RecieveCustomerID(MessageMember member)
        {
            if (member.Message == "edit")
            {
                selectedCustomer = member.Customer;
                CustID = selectedCustomer.CustomerID;
                ModName = selectedCustomer.Name;
                ModAddress = selectedCustomer.Address;
                ModCity = selectedCustomer.City;
                ModZipcode = selectedCustomer.ZipCode;
                ModState = selectedCustomer.State;

                var selected = (from st in MMABooksClass.context.Customers
                                where st.State == modstate
                                select st.State1).FirstOrDefault();
                SelectedState = selected;

            }
        }

        public void AcceptMethod(IClosable window)
        {
            if (ModName != null & ModAddress != null & ModCity != null & SelectedState != null & ModZipcode != null)
            {
                var state = (from cust in MMABooksClass.context.Customers
                             where cust.State1.StateName == SelectedState.StateName
                             select cust.State).FirstOrDefault();
                selectedCustomer.CustomerID = custID;
                selectedCustomer.Name = ModName;
                selectedCustomer.City = ModCity;
                selectedCustomer.Address = ModAddress;
                selectedCustomer.State = state;
                selectedCustomer.ZipCode = ModZipcode;

                Messenger.Default.Send(new MessageMember(selectedCustomer, "Modify"));
                ClearControls();
                if (window != null)
                {
                    window.Close();
                }

                try
                {
                    var modifyCustomer =
                        from c in MMABooksClass.context.Customers
                        where c.CustomerID == custID
                        select c;

                    foreach (Customer customer in modifyCustomer)
                    {

                        customer.Name = ModName;
                        customer.Address = ModAddress;
                        customer.City = ModCity;
                        customer.ZipCode = ModZipcode;
                        customer.State = state;

                    }

                    MMABooksClass.context.SaveChanges();
                    if (window != null)
                    {
                        window.Close();
                    }
                    ClearControls();
        
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ex.Entries.Single().Reload();
                    if (MMABooksClass.context.Entry(selectedstate).State == EntityState.Detached)
                    {
                        MessageBox.Show("Another user has deleted " + "that customer.", "Concurrency Error");
                    }
                    else
                    {
                        MessageBox.Show("Another user has updated " + "that customer.", "Concurrency Error");
                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Null reference", "Error");
                }
                catch (Exception)
                {
                    MessageBox.Show("Error", "Error");
                }

            }
            else
            {
                MessageBox.Show("One or more fields are empty", "Error");
            }
            ClearControls();
            if (window != null)
            {
                window.Close();
            }
        }

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set { selectedCustomer = value; }
        }


        public ObservableCollection<State> States
        {
            get
            {
                return states;
            }
            set
            {
                states = value;
            }
        }

        public State SelectedState
        {
            get
            {
                return selectedstate;
            }
            set
            {
                this.selectedstate = value;
                this.RaisePropertyChanged();
            }
        }

        public void CancelMethod(IClosable window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

        public int CustID
        {
            get
            {
                return custID;
            }
            set
            {
                this.custID = value;
                this.RaisePropertyChanged();
            }
        }

        public string ModName
        {
            get
            {
                return modname;
            }
            set
            {
                this.modname = value;
                this.RaisePropertyChanged();
            }
        }

        public string ModState
        {
            get { return modstate; }
            set
            {
                this.modstate = value;
                this.RaisePropertyChanged();
            }
        }

        public string ModAddress
        {
            get
            {
                return modaddress;
            }
            set
            {
                this.modaddress = value;
                this.RaisePropertyChanged();
            }
        }

        public string ModCity
        {
            get
            {
                return modcity;
            }
            set
            {
                this.modcity = value;
                this.RaisePropertyChanged();
            }
        }

        public string ModZipcode
        {
            get
            {
                return modzipcode;
            }
            set
            {
                this.modzipcode = value;
                this.RaisePropertyChanged();
            }
        }

        private void ClearControls()
        {
            CustID = 0;
            ModName = "";
            ModAddress = "";
            ModCity = "";
            ModState = "";
            ModZipcode = "";
        }

    }
}