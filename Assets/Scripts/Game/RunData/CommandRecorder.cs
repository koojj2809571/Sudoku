using System;
using System.Collections.Generic;
using Game.RunData;
using UI;
using Util;

// ReSharper disable once CheckNamespace
namespace Game
{
    public class CommandRecorder : BaseSingleton<CommandRecorder>
    {
        private readonly Stack<Command> _record = new();
        public readonly Dictionary<int, UINumberInputButton> InputButtons = new();

        public void AddCommand(int index, int value, string key, bool error)
        {
            var command = new Command
            {
                index = index,
                value = value,
                itemKey = key,
                errorCommand = error
            };
            _record.Push(command);
        }

        public void Undo()
        {
            if(!TryGetCommand(out var command)) return;
            var data = NumberRunData.Instance;
            var numberItem = data.dataCtr.numberData[command.index];
            if (numberItem.ItemKey != command.itemKey) return;
            var lastValue = numberItem.Value;
            numberItem.SetValue(command.value);
            data.CurKey = command.itemKey;
            if (InputButtons.TryGetValue(lastValue, out var button))
            {
                button.CheckCanClickCurNum();
            }
        }

        private bool TryGetCommand(out Command command)
        {
            command = null;
            if (_record.Count <= 0) return false;
            command = _record.Pop();
            return true;
        }
    }

    [Serializable]
    public class Command
    {
        public int index;
        public string itemKey;
        public int value;
        public bool errorCommand;
    }
}
