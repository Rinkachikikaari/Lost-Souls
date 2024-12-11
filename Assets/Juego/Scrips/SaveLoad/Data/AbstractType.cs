using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SH
{
    /// <summary>
    /// Abstract class to be referenced by every other class !
    /// Id, and if it is a part of a mod !, it might help to tie things together
    /// </summary>
    public class AbstractType
    {

        public string nameId = "";
        public bool modded = false;

    }

    public class AbstractUIMapping
    {
        public virtual Type mappedType { get; set; }
        public virtual Type mainType { get; set; }
        public virtual AbstractType GetTypeData() { return null; }
    }
}