using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Joeri.Tools
{
    /// <summary>
    /// Classes which can save, and swap two values of one variable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Swapper<T>
    {
        /// <summary>
        /// The value currently within the swapper. Whatever value this one isn't is probably active outside the swapper.
        /// </summary>
        public T holdingValue { get; private set; } 

        /// <summary>
        /// Create a new swapper with a value to swap with.
        /// </summary>
        public Swapper(T originalValue)
        {
            holdingValue = originalValue;
        }

        /// <summary>
        /// Swap the value of a variable with whatever is currently saved within the swapper.
        /// </summary>
        /// <returns>The value that has been swapped out.</returns>
        public T Swap(ref T valueToBeSwapped)
        {
            if (holdingValue == null)       Debug.LogWarning("The holding value is null. Be advised.");
            if (valueToBeSwapped == null)   Debug.LogWarning("The value to be swapped is null. Be advised.");

            var valueToHold = valueToBeSwapped;

            valueToBeSwapped = holdingValue;
            holdingValue = valueToHold;

            return valueToBeSwapped;
        }
    }
}
