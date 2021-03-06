﻿using Qmmands;

namespace Aatrox.Core.Entities
{
    public abstract class AatroxCheckAttribute : CheckAttribute
    {
        public virtual string Name { get; set; } = "Unknown check.";
        public virtual string Details { get; set; } = ": ";
    }
}
