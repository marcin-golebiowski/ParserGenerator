﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserGenerator
{
    public abstract partial class LRkGrammar<Terminal_T, Nonterminal_T>
    {
        public class LRItem
        {
            public ProductionRule Rule { get; }
            public int Marker { get; }
            public bool IsKernel { get; }
            public int Length => Rule.Length;
            public HashSet<Terminal_T> Lookaheads { get; }

            public LRItem(ProductionRule rule, int marker, IEnumerable<Terminal_T> lookaheads = null, bool? isKernel = null)
            {
                Rule = rule;
                Marker = marker;
                IsKernel = isKernel ?? marker != 0;
                Lookaheads = new HashSet<Terminal_T>(lookaheads ?? Enumerable.Empty<Terminal_T>());
            }

            int? hash;
            public override int GetHashCode()
            {
                if (!hash.HasValue)
                {
                    hash = 17;
                    hash = 23 * hash + Marker;
                    hash = 23 * hash + Rule.GetHashCode();
                }
                return hash.Value;
            }

            public override bool Equals(object obj)
            {
                var t = obj as LRItem;

                return t?.Marker == Marker && ReferenceEquals(Rule, t.Rule);
            }

            public override string ToString()
            {
                var sb = new StringBuilder();
            
                for (var i = 0; i < Length; i++)
                {
                    var marker = (i == Marker ? " *" : "");

                    sb.Append($"{marker} {Rule.Symbols[i]}");
                }

                if (Marker >= Length)
                    sb.Append(" *");

                if (Lookaheads.Count > 0)
                {
                    var lookaheads = string.Join(", ", Lookaheads);
                    sb.Append($"({lookaheads})");
                }

                return $"{Rule.Production} ->{sb}";
            }
        }
    }
}
