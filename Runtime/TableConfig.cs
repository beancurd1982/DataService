using System;
using static Yang.Data.ITableConfigRead;

namespace Yang.Data
{
    /// <summary>
    /// Write interface (internal). Only friend assemblies (via InternalsVisibleTo)
    /// can see/consume this.
    /// </summary>
    internal interface ITableConfigWrite
    {
        void SetName(string name);
        void SetAge(int age);
        void SetNameAndAge(string name, int age);
    }

    /// <summary>
    /// Public read-only interface (for UI/consumers that must not mutate data).
    /// </summary>
    public interface ITableConfigRead
    {
        string Name { get; }
        int Age { get; }

        event EventHandler<NameChangedEvent> NameChanged;
        event EventHandler<AgeChangedEvent> AgeChanged;

        public readonly struct NameChangedEvent
        {
            public readonly string OldName;
            public readonly string NewName;

            public NameChangedEvent(string oldName, string newName)
            {
                OldName = oldName;
                NewName = newName;
            }
        }

        public readonly struct AgeChangedEvent
        {
            public readonly int OldAge;
            public readonly int NewAge;

            public AgeChangedEvent(int oldAge, int newAge)
            {
                OldAge = oldAge;
                NewAge = newAge;
            }
        }
    }

    public sealed class TableConfig : ITableConfigRead, ITableConfigWrite
    {
        private string _name;
        private int _age;

        private event EventHandler<NameChangedEvent> _nameChanged;
        private event EventHandler<AgeChangedEvent> _ageChanged;

        /// <summary>
        /// Public read-only view for any consumer (UI, etc.).
        /// </summary>
        public ITableConfigRead AsRead() => this;

        /// <summary>
        /// Internal write view. Requires InternalsVisibleTo on this assembly
        /// to be visible to the writer assembly.
        /// </summary>
        internal ITableConfigWrite AsWrite() => this;

        #region ITableConfigRead (explicit implementation; hidden from typical call sites)
        event EventHandler<NameChangedEvent> ITableConfigRead.NameChanged
        {
            add => _nameChanged += value;
            remove => _nameChanged -= value;
        }

        event EventHandler<AgeChangedEvent> ITableConfigRead.AgeChanged
        {
            add => _ageChanged += value;
            remove => _ageChanged -= value;
        }

        string ITableConfigRead.Name => _name;
        int ITableConfigRead.Age => _age;
        #endregion

        #region ITableConfigWrite (explicit implementation; hidden from typical call sites)
        void ITableConfigWrite.SetName(string name)
        {
            if (_name == name) return;
            var old = _name;
            _name = name;
            _nameChanged?.Invoke(this, new ITableConfigRead.NameChangedEvent(old, name));
        }

        void ITableConfigWrite.SetAge(int age)
        {
            if (_age == age) return;
            var old = _age;
            _age = age;
            _ageChanged?.Invoke(this, new ITableConfigRead.AgeChangedEvent(old, age));
        }

        void ITableConfigWrite.SetNameAndAge(string name, int age)
        {
            // Reuse the same logic to keep event ordering/dedup consistent.
            ((ITableConfigWrite)this).SetName(name);
            ((ITableConfigWrite)this).SetAge(age);
        }
        #endregion
    }
}