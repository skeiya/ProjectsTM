using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Service
{
    enum DragState
    {
        None,
        BeforeExpanding,
        Expanding,
        BeforeMoving,
        Moving,
        Copying,
    }
}
