﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DunKanren.Goals
{
    internal abstract class Relation : Goal<ADT.Term>
    {
        protected ADT.Term LeftArg;
        protected ADT.Term RightArg;

        public override Stream PursueIn(State s)
        {
            return base.PursueIn(s);
        }

        protected Relation(ADT.Term arg1, ADT.Term arg2) : base()
        {
            this.LeftArg = arg1;
            this.RightArg = arg2;

            this.Subs = new() { this.LeftArg, this.RightArg };
        }

        //public override IEnumerable<string> ToTree(string prefix, bool first, bool last)
        //{
        //    string parentPrefix = first ? "" : prefix + (last ? IO.LEAVES : IO.BRANCH);
        //    string childPrefix = first ? "" : prefix + (last ? IO.SPACER : IO.JUMPER);

        //    yield return parentPrefix + (this.SubExpressions.Any() ? IO.HEADER : IO.ALONER) + this.Description;

        //    foreach (string line in this.LeftArg.ToTree(childPrefix, false, false)) yield return line;

        //    foreach (string line in this.RightArg.ToTree(childPrefix, false, true)) yield return line;

        //    //if (this.SubExpressions.Any())
        //    //{
        //    //    foreach (var comp in this.SubExpressions.SkipLast(1))
        //    //    {
        //    //        foreach (string line in comp.ToTree(childPrefix, false, false))
        //    //        {
        //    //            yield return line;
        //    //        }
        //    //    }

        //    //    foreach (string line in this.SubExpressions.Last().ToTree(childPrefix, false, true))
        //    //    {
        //    //        yield return line;
        //    //    }
        //    //}
        //}

        //public override uint Ungroundedness => uint.Min(LeftArg.Ungroundedness, RightArg.Ungroundedness);
    }

    /// <summary>
    /// Represents the unification of two ADT.Terms
    /// </summary>
    internal class Equality : Relation
    {
        public override string Expression => $"{this.LeftArg} ≡ {this.RightArg}";
        public override string Description => "The following ADT.Terms are equivalent";
        public override IEnumerable<IPrintable> SubExpressions => new[] { this.LeftArg, this.RightArg };

        public Equality(ADT.Term lhs, ADT.Term rhs) : base(lhs, rhs) { }

        internal override Lazy<Func<State, Stream>> GetApp() => new(() => (State s) => Equality.Assert(s, this.LeftArg, this.RightArg));
        internal override Lazy<Func<State, Stream>> GetNeg() => new(() => (State s) => Disequality.Assert(s, this.LeftArg, this.RightArg));

        public static Stream Assert(State s, ADT.Term left, ADT.Term right)
        {
            //if (s.TryUnify(left, right, out State result))
            //{
            //    return Stream.Singleton(result);
            //}
            //return Stream.Empty();
            return left.Unify(right, s);
        }
    }

    /// <summary>
    /// Represents the dis-unification of two ADT.Terms
    /// </summary>
    internal class Disequality : Relation
    {
        public override string Expression => String.Join(" != ", this.SubExpressions);
        public override string Description => "The following ADT.Terms are NOT equivalent";

        public Disequality(ADT.Term lhs, ADT.Term rhs) : base(lhs, rhs) { }

        internal override Lazy<Func<State, Stream>> GetApp() => new(() => (State s) => Disequality.Assert(s, this.LeftArg, this.RightArg));
        internal override Lazy<Func<State, Stream>> GetNeg() => new(() => (State s) => Equality.Assert(s, this.LeftArg, this.RightArg));

        public static Stream Assert(State s, ADT.Term left, ADT.Term right)
        {
            //if (s.TryDisUnify(left, right, out State result))
            //{
            //    return Stream.Singleton(result);
            //}
            //return Stream.Empty();
            return left.DisUnify(right, s);
        }
    }
}
