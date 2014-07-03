using System;
using System.Collections.Generic;
using System.Drawing;
using Substrate.TileEntities;

namespace SeeSharp
{

    abstract public class SignBase
    {
        /// <summary>
        ///     Where in the world the sign is, X/Z.
        /// </summary>
        protected internal Point Location;

        /// <summary>
        ///     Simple string denoting Sign type
        /// </summary>
        /// <returns>
        ///     The sign header, sans square brackets.
        /// </returns>
        /// <remarks>
        ///     Refer to the DefaultSigns source code for how to implement this function
        /// </remarks>
        abstract protected internal string SignType();

        /// <summary>
        ///     Given a sign, create an object of this sign type.  Return False if the sign does not meet creation requirements
        /// </summary>
        /// <param name="BaseEntity">
        ///     The TileEntity of the base sign
        /// </param>
        /// <returns>
        ///     True if the sign can be created, False if it cannot
        /// </returns>
        /// <remarks>
        ///     This function should not perform extensive validation, as it is possible for multiple signs to be included via AddSign().
        /// </remarks>
        abstract protected internal bool CreateFrom(TileEntitySign BaseEntity);

        /// <summary>
        ///     Given a subsequent sign after creation, determine if the sign is a valid member of the struct.
        /// </summary>
        /// <param name="Sign">
        ///     The TileEntity of the sign
        /// </param>
        /// <returns>
        ///     True if the sign data is valid and this object should continue reading signs from the world
        ///     False if the sign is not valid and this object's setup should be completed.
        /// </returns>
        abstract protected internal bool AddSign(TileEntitySign Sign);

        /// <summary>
        ///     Returns whether the sign is valid or not depending on sign-specific logic
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///     If False is returned, the sign will not appear in the exported XML
        /// </remarks>
        abstract protected internal bool IsValid();

        /// <summary>
        ///     Collect and return a list of KeyValuePair objects with the data to be serialized to XML.
        /// </summary>
        /// <param name="Parameters">Parameters List.  An instantiated list will be passed in for you.</param>
        abstract protected internal void ExportParameters(List<KeyValuePair<String, String>> Parameters);

        /// <summary>
        ///     Utility Function to generate a Key-Value pair for ExportParameters
        /// </summary>
        /// <param name="S1">
        ///     The string to use as the Key
        /// </param>
        /// <param name="S2">
        ///     The string to use as the Value
        /// </param>
        /// <returns></returns>
        protected KeyValuePair<String, String> Pair(String S1, String S2)
        {
            return new KeyValuePair<string, string>(S1, S2);
        }
    }
}