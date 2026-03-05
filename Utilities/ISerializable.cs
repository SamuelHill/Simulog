using System;

namespace Utilities {
    /// <summary> Interface used to indicate that a class is serializable (i.e. has a ToString method) </summary>
    public interface ISerializableValue {
        /// <summary> ToString method to be overridden for serializable objects </summary>
        /// <returns> a string representing the object </returns>
        public string ToString();
    }

    /// <summary> Interface used to indicate that a class is serializable (i.e. has a ToString and FromString method) </summary>
    /// <typeparam name="T">Type of object to be serialized</typeparam>
    public interface ISerializableValue<out T> : ISerializableValue {
        /// <summary> For use by CsvReader. Takes a string and converts it to an object of type T. </summary>
        /// <param name="fromString">String containing a serialized object of type T.</param>
        /// <returns>object of type T deserialized from the input string</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static T FromString(string fromString) {
            throw new NotImplementedException();
        }
    }

    /// <summary> Interface used to indicate that a class is serializable (i.e. has a ToString and FromString method) </summary>
    /// <typeparam name="T">Type of object to be serialized</typeparam>
    public interface ISerializableRandomValue<out T> : ISerializableValue {
        /// <summary> For use by CsvReader. Takes a string and converts it to an object of type T. </summary>
        /// <param name="fromString">String containing a serialized object of type T.</param>
        /// <param name="rng">RNG used to create this object</param>
        /// <returns>object of type T deserialized from the input string</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static T FromString(string fromString, Random rng) {
            throw new NotImplementedException();
        }
    }
}
