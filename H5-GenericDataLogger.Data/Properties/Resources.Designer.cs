﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace H5_GenericDataLogger.Data.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("H5_GenericDataLogger.Data.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS log_entries (id INTEGER PRIMARY KEY, log_id INTEGER REFERENCES logs(id));
        ///.
        /// </summary>
        internal static string sql_ensure_log_entries {
            get {
                return ResourceManager.GetString("sql_ensure_log_entries", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS log_fields (id INTEGER PRIMARY KEY, log_id INTEGER REFERENCES logs(id), label TEXT, data_type INTEGER);
        ///.
        /// </summary>
        internal static string sql_ensure_log_fields {
            get {
                return ResourceManager.GetString("sql_ensure_log_fields", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS integer_log_values (
        ///	id INTEGER PRIMARY KEY,
        ///	entry_id INTEGER REFERENCES log_entries(id),
        ///	field_id INTEGER REFERENCES log_fields(id),
        ///	val INTEGER
        ///);
        ///
        ///CREATE TABLE IF NOT EXISTS real_log_values (
        ///	id INTEGER PRIMARY KEY,
        ///	entry_id INTEGER REFERENCES log_entries(id),
        ///	field_id INTEGER REFERENCES log_fields(id),
        ///	val REAL
        ///);
        ///
        ///CREATE TABLE IF NOT EXISTS text_log_values (
        ///	id INTEGER PRIMARY KEY,
        ///	entry_id INTEGER REFERENCES log_entries(id),
        ///	field_id INTEGER REFER [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string sql_ensure_log_values {
            get {
                return ResourceManager.GetString("sql_ensure_log_values", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE TABLE IF NOT EXISTS logs (id INTEGER PRIMARY KEY, title TEXT);.
        /// </summary>
        internal static string sql_ensure_logs {
            get {
                return ResourceManager.GetString("sql_ensure_logs", resourceCulture);
            }
        }
    }
}
