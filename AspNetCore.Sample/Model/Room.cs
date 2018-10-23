﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Sample.Model
{
  public class Room
  {
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }

   
    public string Guid { get; set; }
    public string Description { get; set; }
    public double Area { get; set; }

  }
}

