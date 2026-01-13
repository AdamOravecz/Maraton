using System;
using System.Collections.Generic;

namespace Maraton_webAPI.Models;

public partial class Eredmenyek
{
    public int Futo { get; set; }

    public int Kor { get; set; }

    public int Ido { get; set; }

    public virtual Futok FutoNavigation { get; set; } = null!;
}
