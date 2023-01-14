﻿using System;
using System.Collections.Generic;
using Commands;
using HECSFramework.Core;

namespace Components
{
    [Documentation(Doc.HECS, Doc.Counters, "this component is base for all counters components with modifiable values. this component holds modifier container")]
    public abstract partial class ModifiableFloatCounterComponent : BaseComponent, ICounterModifiable<float>, IInitable, IDisposable
    {
        public float Value => modifiableIntCounter.Value;
        public float CalculatedMaxValue => modifiableIntCounter.CalculatedMaxValue;
        public abstract int Id { get; }
        public abstract float SetupValue { get; }

        protected ModifiableFloatCounter modifiableIntCounter = new ModifiableFloatCounter();
        protected bool isReactive;

        public bool IsReactive { get => isReactive; protected set => isReactive = value; }

        public void Init()
        {
            modifiableIntCounter.Setup(Id, SetupValue);
        }

        public void AddModifier(Guid owner, IModifier<float> modifier) 
        {
            var oldValue = Value;
            modifiableIntCounter.AddModifier(owner, modifier);

            if (isReactive)
                Owner.Command(GetDiffCommand(oldValue));
        } 

        public void RemoveModifier(Guid owner, IModifier<float> modifier) 
        {
            var oldValue = Value;
            modifiableIntCounter.RemoveModifier(owner, modifier);

            if (isReactive)
                Owner.Command(GetDiffCommand(oldValue));
        } 

        public void AddUniqueModifier(Guid owner, IModifier<float> modifier)
        {
            var oldValue = Value;
            modifiableIntCounter.AddUniqueModifier(owner, modifier);

            if (isReactive)
                Owner.Command(GetDiffCommand(oldValue));
        }

        private DiffCounterCommand<float> GetDiffCommand(float oldValue)
        {
            return new DiffCounterCommand<float>
            {
                Id = this.Id,
                Value = modifiableIntCounter.Value,
                PreviousValue = oldValue,
                MaxValue = modifiableIntCounter.CalculatedMaxValue
            };
        }

        public void SetReactive(bool state)
        {
            IsReactive = state;
        }

        public void SetValue(float value)
        {
            var oldValue = Value;
            modifiableIntCounter.SetValue(value);

            if (IsReactive && CheckModifiedDiff(oldValue, out var command))
                Owner.Command(command);
        }

        public void ChangeValue(float value)
        {
            var oldValue = Value;

            modifiableIntCounter.ChangeValue(value);

            if (IsReactive && CheckModifiedDiff(oldValue, out var command))
                Owner.Command(command);
        }

        private bool CheckModifiedDiff(float oldValue, out DiffCounterCommand<float> result)
        {
            if (oldValue != Value)
            {
                result = new DiffCounterCommand<float> { Id = this.Id, Value = modifiableIntCounter.Value, PreviousValue = oldValue, MaxValue = modifiableIntCounter.CalculatedMaxValue };
                return true;
            }

            result = default;
            return false;
        }

        public void Dispose()
        {
            modifiableIntCounter.Dispose();
        }

        public void Reset()
        {
            modifiableIntCounter.Reset();
        }

        public IEnumerable<IModifier<float>> GetModifiers()
        {
            throw new NotImplementedException();
        }
    }
}