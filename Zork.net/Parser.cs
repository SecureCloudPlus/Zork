﻿using System;
using Zork.Core.Clock;
using Zork.Core.Object;
using Zork.Core.Room;

namespace Zork.Core
{
    public static class Parser
    {
        public static string ReadLine(int who)
        {
            string buffer;

            switch (who + 1)
            {
                case 1: goto L90;
                case 2: goto L10;
            }

        L10:
            Console.Write(">");

        L90:
            buffer = Console.ReadLine();
            if (buffer.Length > 1)
                if (buffer[0] == '!')
                {
                    buffer = buffer.TrimStart(new[] { '!' });
                }

            return buffer.ToUpper() + '\0';
        }

        /// <summary>
        /// parse_ - Top level parse routine
        /// </summary>
        /// <param name="input"></param>
        /// <param name="vbflag"></param>
        public static bool Parse(string input, bool vbflag, Game game)
        {
            int i__1;
            bool ret_val;

            int[] outbuf = new int[40];
            int outlnt;

            // Parameter adjustments
            //--input;

            // Function Body
            ret_val = false;

            // !ASSUME FAILS.
            game.ParserVectors.prsa = 0;

            // !ZERO OUTPUTS.
            game.ParserVectors.prsi = 0;
            game.ParserVectors.prso = 0;

            if (!Parser.Lex(input, outbuf, out outlnt, vbflag, game))
            {
                goto L100;
            }

            if ((i__1 = sparse_(outbuf, outlnt, vbflag, game)) < 0)
            {
                goto L100;
            }
            else if (i__1 == 0)
            {
                goto L200;
            }
            else
            {
                goto L300;
            }

        // !DO SYN SCAN.

        // PARSE REQUIRES VALIDATION

        L200:
            if (!(vbflag))
            {
                goto L350;
            }
            // !ECHO MODE, FORCE FAIL.
            if (!synmch_(game))
            {
                goto L100;
            }

            // !DO SYN MATCH.
            if (game.ParserVectors.prso > 0 & game.ParserVectors.prso < (int)XSearch.xmin)
            {
                game.Last.lastit = game.ParserVectors.prso;
            }

        // SUCCESSFUL PARSE OR SUCCESSFUL VALIDATION

        L300:
            ret_val = true;
        L350:
            Orphans.Orphan(0, 0, 0, 0, 0, game);

            // !CLEAR ORPHANS.
            return ret_val;

        // PARSE FAILS, DISALLOW CONTINUATION

        L100:
            game.ParserVectors.prscon = 1;

            return ret_val;
        }

        /// <summary>
        /// Lex_ - lexical analyzer
        /// </summary>
        /// <param name="inbuf"></param>
        /// <param name="outbuf"></param>
        /// <param name="op"></param>
        /// <param name="vbflag"></param>
        /// <returns></returns>
        public static bool Lex(string inbuf, int[] outbuf, out int op, bool vbflag, Game game)
        {
            char[] dlimit = new char[9] { 'A', 'Z', (char)('A' - 1), '1', '9', (char)('1' - 31), '-', '-', (char)('-' - 27) };
            bool ret_val = false;

            int i;
            char j;
            int k = 0, j1, j2, cp;

            // Parameter adjustments
            // --outbuf;
            // --inbuf;

            // !ASSUME LEX FAILS.
            op = -1;

        // !OUTPUT PTR.
        L50:
            op += 2;

            // !ADV OUTPUT PTR.
            cp = 0;

        // !CHAR PTR=0.

        L200:
            j = inbuf[game.ParserVectors.prscon - 1];

            // !GET CHARACTER
            if (j == '\0')
            {
                goto L1000;
            }

            // !END OF INPUT?

            ++game.ParserVectors.prscon;
            // !ADVANCE PTR.

            if (j == '.')
            {
                goto L1000;
            }

            // !END OF COMMAND?
            if (j == ',')
            {
                goto L1000;
            }

            // !END OF COMMAND?
            if (j == ' ')
            {
                goto L6000;
            }

            // !SPACE?
            for (i = 1; i <= 9; i += 3)
            {
                // !SCH FOR CHAR.
                if (j >= dlimit[i - 1] & j <= dlimit[i])
                {
                    goto L4000;
                }
                // L500:
            }

            if (vbflag)
            {
                MessageHandler.Speak(601, game);
            }

            // !GREEK TO ME, FAIL.
            return ret_val;

        // END OF INPUT, SEE IF PARTIAL WORD AVAILABLE.

        L1000:
            if (inbuf[game.ParserVectors.prscon - 1] == '\0')
            {
                game.ParserVectors.prscon = 1;
            }

            // !FORCE PARSE RESTART.
            if (cp == 0 & op == 1)
            {
                return ret_val;
            }

            if (cp == 0)
            {
                op += -2;
            }

            // !ANY LAST WORD?
            ret_val = true;
            return ret_val;

        // LEGITIMATE CHARACTERS: LETTER, DIGIT, OR HYPHEN.

        L4000:
            j1 = j - dlimit[i + 1];
            if (cp >= 6)
            {
                goto L200;
            }

            // !IGNORE IF TOO MANY CHAR.
            k = op + cp / 3;

            // !COMPUTE WORD INDEX.
            switch (cp % 3 + 1)
            {
                case 1: goto L4100;
                case 2: goto L4200;
                case 3: goto L4300;
            }

        // !BRANCH ON CHAR.
        L4100:
            j2 = j1 * 780;

            // !CHAR 1... *780
            outbuf[k] = outbuf[k] + j2 + j2;

        // !*1560 (40 ADDED BELOW).
        L4200:
            outbuf[k] += j1 * 39;

        // !*39 (1 ADDED BELOW).
        L4300:
            outbuf[k] += j1;

            // !*1.
            ++cp;
            goto L200;
        // !GET NEXT CHAR.

        // SPACE

        L6000:
            if (cp == 0)
            {
                goto L200;
            }

            // !ANY WORD YET?
            goto L50;
            // !YES, ADV OP.
        }

        /// <summary>
        /// THIS ROUTINE DETAILS ON BIT 4 OF PRSFLG
        /// </summary>
        private static bool synmch_(Game game)
        {
            //  THE FOLLOWING DATA STATEMENT WAS ORIGINALLY:
            // 	DATA R50MIN/1RA/

            const int r50min = 1600;

            // System generated locals
            int i__1;
            bool ret_val;

            // Local variables
            int j;
            int newj;
            int drive, limit, qprep, sprep, dforce;

            ret_val = false;
            j = game.pv_1.act;
            // !SET UP PTR TO SYNTAX.
            drive = 0;
            // !NO DEFAULT.
            dforce = 0;
            // !NO FORCED DEFAULT.
            qprep = game.Orphans.oflag & game.Orphans.oprep;
        L100:
            j += 2;

            // !FIND START OF SYNTAX.
            if (ParserConstants.vvoc[j - 1] <= 0 || ParserConstants.vvoc[j - 1] >= r50min)
            {
                goto L100;
            }

            limit = j + ParserConstants.vvoc[j - 1] + 1;
            // !COMPUTE LIMIT.
            ++j;
        // !ADVANCE TO NEXT.

        L200:
            unpack_(j, out newj, game);
            // !UNPACK SYNTAX.
            sprep = game.Syntax.dobj & (int)SyntaxObjectFlags.VPMASK;
            if (!syneql_(game.prpvec.p1, game.objvec.o1, game.Syntax.dobj, game.Syntax.dfl1, game.Syntax.dfl2, game))
            {
                goto L1000;
            }

            sprep = game.Syntax.iobj & (int)SyntaxObjectFlags.VPMASK;
            if (syneql_(game.prpvec.p2, game.objvec.o2, game.Syntax.iobj, game.Syntax.ifl1, game.Syntax.ifl2, game))
            {
                goto L6000;
            }

            // SYNTAX MATCH FAILS, TRY NEXT ONE.

            if (game.objvec.o2 != 0)
            {
                goto L3000;
            }
            else
            {
                goto L500;
            }

        // !IF O2=0, SET DFLT.
        L1000:
            if (game.objvec.o1 != 0)
            {
                goto L3000;
            }
            else
            {
                goto L500;
            }

        // !IF O1=0, SET DFLT.
        L500:
            if (qprep == 0 || qprep == sprep)
            {
                dforce = j;
            }

            // !IF PREP MCH.
            if ((game.Syntax.vflag & SyntaxFlags.SDRIV) != 0)
            {
                drive = j;
            }

        L3000:
            j = newj;
            if (j < limit)
            {
                goto L200;
            }
            // !MORE TO DO?
            // SYNMCH, PAGE 2

            // MATCH HAS FAILED.  IF DEFAULT SYNTAX EXISTS, TRY TO SNARF
            // ORPHANS OR GWIMS, OR MAKE NEW ORPHANS.

            if (drive == 0)
            {
                drive = dforce;
            }

            // !NO DRIVER? USE FORCE.
            if (drive == 0)
            {
                goto L10000;
            }

            // !ANY DRIVER?
            unpack_(drive, out dforce, game);
            // !UNPACK DFLT SYNTAX.

            // TRY TO FILL DIRECT OBJECT SLOT IF THAT WAS THE PROBLEM.
            if (game.Syntax.vflag.HasFlag(SyntaxFlags.SDIR) || game.objvec.o1 != 0)
            {
                goto L4000;
            }

            // FIRST TRY TO SNARF ORPHAN OBJECT.
            game.objvec.o1 = game.Orphans.oflag & game.Orphans.oslot;
            if (game.objvec.o1 == 0)
            {
                goto L3500;
            }

            // !ANY ORPHAN?
            if (syneql_(game.prpvec.p1, game.objvec.o1, game.Syntax.dobj, game.Syntax.dfl1, game.Syntax.dfl2, game))
            {
                goto L4000;
            }

        // ORPHAN FAILS, TRY GWIM.
        L3500:
            game.objvec.o1 = gwim_(game.Syntax.dobj, game.Syntax.dfw1, game.Syntax.dfw2, game);
            // !GET GWIM.
            if (game.objvec.o1 > 0)
            {
                goto L4000;
            }

            // !TEST RESULT.
            i__1 = game.Syntax.dobj & (int)SyntaxObjectFlags.VPMASK;
            Orphans.Orphan(-1, game.pv_1.act, 0, i__1, 0, game);
            MessageHandler.Speak(623, game);
            return ret_val;

        // TRY TO FILL INDIRECT OBJECT SLOT IF THAT WAS THE PROBLEM.

        L4000:
            if ((game.Syntax.vflag & SyntaxFlags.SIND) == 0 || game.objvec.o2 != 0)
            {
                goto L6000;
            }

            game.objvec.o2 = gwim_(game.Syntax.iobj, game.Syntax.ifw1, game.Syntax.ifw2, game);
            // !GWIM.
            if (game.objvec.o2 > 0)
            {
                goto L6000;
            }
            if (game.objvec.o1 == 0)
            {
                game.objvec.o1 = game.Orphans.oflag & game.Orphans.oslot;
            }
            i__1 = game.Syntax.dobj & (int)SyntaxObjectFlags.VPMASK;
            Orphans.Orphan(-1, game.pv_1.act, game.objvec.o1, i__1, 0, game);
            MessageHandler.Speak(624, game);
            return ret_val;

        // TOTAL CHOMP

        L10000:
            MessageHandler.Speak(601, game);
            // !CANT DO ANYTHING.
            return ret_val;
        // SYNMCH, PAGE 3

        // NOW TRY TO TAKE INDIVIDUAL OBJECTS AND
        // IN GENERAL CLEAN UP THE PARSE VECTOR.

        L6000:
            if ((game.Syntax.vflag & SyntaxFlags.SFLIP) == 0)
            {
                goto L5000;
            }
            j = game.objvec.o1;
            // !YES.
            game.objvec.o1 = game.objvec.o2;
            game.objvec.o2 = j;

        L5000:
            game.ParserVectors.prsa = (int)(game.Syntax.vflag & SyntaxFlags.SVMASK);
            game.ParserVectors.prso = game.objvec.o1;
            // !GET DIR OBJ.
            game.ParserVectors.prsi = game.objvec.o2;
            // !GET IND OBJ.
            if (!takeit_(game.ParserVectors.prso, game.Syntax.dobj, game))
            {
                return ret_val;
            }
            // !TRY TAKE.
            if (!takeit_(game.ParserVectors.prsi, game.Syntax.iobj, game))
            {
                return ret_val;
            }
            // !TRY TAKE.
            ret_val = true;
            return ret_val;
        }

        /// <summary>
        /// gwim_ - Get what I mean
        /// </summary>
        /// <param name="sflag"></param>
        /// <param name="sfw1"></param>
        /// <param name="sfw2"></param>
        /// <returns></returns>
        private static int gwim_(int sflag, int sfw1, int sfw2, Game game)
        {
            // System generated locals
            int ret_val;

            // Local variables
            int av;
            int nobj, robj;
            bool nocare;

            // GWIM, PAGE 2

            ret_val = -1;
            // !ASSUME LOSE.
            av = game.Adventurers.Vehicles[game.Player.Winner - 1];
            nobj = 0;
            nocare = (sflag & (int)SyntaxObjectFlags.VCBIT) == 0;

            // FIRST SEARCH ADVENTURER

            if ((sflag & (int)SyntaxObjectFlags.VABIT) != 0)
            {
                nobj = fwim_(sfw1, sfw2, 0, 0, game.Player.Winner, nocare, game);
            }

            if ((sflag & (int)SyntaxObjectFlags.VRBIT) != 0)
            {
                goto L100;
            }

        L50:
            ret_val = nobj;
            return ret_val;

        // ALSO SEARCH ROOM

        L100:
            robj = fwim_(sfw1, sfw2, game.Player.Here, 0, 0, nocare, game);
            if (robj < 0)
            {
                goto L500;
            }
            else if (robj == 0)
            {
                goto L50;
            }
            else
            {
                goto L200;
            }
        // !TEST RESULT.

        // ROBJ > 0

        L200:
            if (av == 0 || robj == av || (game.Objects.oflag2[robj - 1] & ObjectFlags2.FINDBT)
                != 0)
            {
                goto L300;
            }
            if (game.Objects.ocan[robj - 1] != av)
            {
                goto L50;
            }
        // !UNREACHABLE? TRY NOBJ
        L300:
            if (nobj != 0)
            {
                return ret_val;
            }
            // !IF AMBIGUOUS, RETURN.
            if (!takeit_(robj, sflag, game))
            {
                return ret_val;
            }
            // !IF UNTAKEABLE, RETURN
            ret_val = robj;
        L500:
            return ret_val;
        }

        /// <summary>
        /// fwim_ - Find what I mean
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="rm"></param>
        /// <param name="con"></param>
        /// <param name="adv"></param>
        /// <param name="nocare"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public static int fwim_(int f1, int f2, int rm, int con, int adv, bool nocare, Game game)
        {
            int ret_val, i__1, i__2;

            // Local variables
            int i, j;

            // OBJECTS
            ret_val = 0;
            // !ASSUME NOTHING.
            i__1 = game.Objects.Count;
            for (i = 1; i <= i__1; ++i)
            {
                // !LOOP
                if ((rm == 0 || game.Objects.oroom[i - 1] != rm) && (adv == 0 ||
                    game.Objects.oadv[i - 1] != adv) && (con == 0 || game.Objects.ocan[i - 1] != con))
                {
                    goto L1000;
                }

                // OBJECT IS ON LIST... IS IT A MATCH?

                if ((game.Objects.oflag1[i - 1] & ObjectFlags.VISIBT) == 0)
                {
                    goto L1000;
                }

                // double check: was ~(nocare)
                if (!nocare & (game.Objects.oflag1[i - 1] & ObjectFlags.TAKEBT) == 0
                    || ((int)game.Objects.oflag1[i - 1] & f1) == 0 && ((int)game.Objects.oflag2[i - 1] & f2) == 0)
                {
                    goto L500;
                }
                if (ret_val == 0)
                {
                    goto L400;
                }
                // !ALREADY GOT SOMETHING?
                ret_val = -ret_val;
                // !YES, AMBIGUOUS.
                return ret_val;

            L400:
                ret_val = i;
            // !NOTE MATCH.

            // DOES OBJECT CONTAIN A MATCH?

            L500:
                if ((game.Objects.oflag2[i - 1] & ObjectFlags2.OPENBT) == 0)
                {
                    goto L1000;
                }

                i__2 = game.Objects.Count;
                for (j = 1; j <= i__2; ++j)
                {
                    // !NO, SEARCH CONTENTS.
                    if (game.Objects.ocan[j - 1] != i
                        || (game.Objects.oflag1[j - 1] & ObjectFlags.VISIBT) == 0
                        || ((int)game.Objects.oflag1[j - 1] & f1) == 0
                        && ((int)game.Objects.oflag2[j - 1] & f2) == 0)
                    {
                        goto L700;
                    }
                    if (ret_val == 0)
                    {
                        goto L600;
                    }
                    ret_val = -ret_val;
                    return ret_val;

                L600:
                    ret_val = j;
                L700:
                    ;
                }
            L1000:
                ;
            }
            return ret_val;
        }

        /// <summary>
        /// takeit_ - PARSER BASED TAKE OF OBJECT
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        private static bool takeit_(int obj, int sflag, Game game)
        {
            bool ret_val;

            // Local variables
            int x;
            int odo2;

            // TAKEIT, PAGE 2

            ret_val = false;
            // !ASSUME LOSES.
            if (obj == 0 || obj > game.Star.strbit)
            {
                goto L4000;
            }

            // !NULL/STARS WIN.
            odo2 = game.Objects.odesc2[obj - 1];
            // !GET DESC.
            x = game.Objects.ocan[obj - 1];
            // !GET CONTAINER.
            if (x == 0 || (sflag & (int)SyntaxObjectFlags.VFBIT) == 0)
            {
                goto L500;
            }
            if ((game.Objects.oflag2[x - 1] & ObjectFlags2.OPENBT) != 0)
            {
                goto L500;
            }

            MessageHandler.rspsub_(566, odo2, game);
            // !CANT REACH.
            return ret_val;

        L500:
            if ((sflag & (int)SyntaxObjectFlags.VRBIT) == 0)
            {
                goto L1000;
            }
            if ((sflag & (int)SyntaxObjectFlags.VTBIT) == 0)
            {
                goto L2000;
            }

            // SHOULD BE IN ROOM (VRBIT NE 0) AND CAN BE TAKEN (VTBIT NE 0)

            if (schlst_(0, 0, game.Player.Here, 0, 0, obj, game) <= 0)
            {
                goto L4000;
            }
            // !IF NOT, OK.

            // ITS IN THE ROOM AND CAN BE TAKEN.

            if ((game.Objects.oflag1[obj - 1] & ObjectFlags.TAKEBT) != 0
                && (game.Objects.oflag2[obj - 1] & ObjectFlags2.TRYBT) == 0)
            {
                goto L3000;
            }

            // NOT TAKEABLE.  IF WE CARE, FAIL.

            if ((sflag & (int)SyntaxObjectFlags.VCBIT) == 0)
            {
                goto L4000;
            }

            MessageHandler.rspsub_(445, odo2, game);
            return ret_val;

        // 1000--	IT SHOULD NOT BE IN THE ROOM.
        // 2000--	IT CANT BE TAKEN.

        L2000:
            if ((sflag & (int)SyntaxObjectFlags.VCBIT) == 0)
            {
                goto L4000;
            }
        L1000:
            if (schlst_(0, 0, game.Player.Here, 0, 0, obj, game) <= 0)
            {
                goto L4000;
            }

            MessageHandler.rspsub_(665, odo2, game);

            return ret_val;
        // TAKEIT, PAGE 3

        // OBJECT IS IN THE ROOM, CAN BE TAKEN BY THE PARSER,
        // AND IS TAKEABLE IN GENERAL.  IT IS NOT A STAR.
        // TAKING IT SHOULD NOT HAVE SIDE AFFECTS.
        // IF IT IS INSIDE SOMETHING, THE CONTAINER IS OPEN.
        // THE FOLLOWING CODE IS LIFTED FROM SUBROUTINE TAKE.

        L3000:
            if (obj != game.Adventurers.Vehicles[game.Player.Winner - 1])
            {
                goto L3500;
            }
            // !TAKE VEHICLE?
            MessageHandler.Speak(672, game);
            return ret_val;

        L3500:
            if (x != 0 && game.Objects.oadv[x - 1] == game.Player.Winner || ObjectHandler.weight_(0, obj, game.Player.Winner, game) + game.Objects.osize[obj - 1] <= game.State.MaxLoad)
            {
                goto L3700;
            }

            MessageHandler.Speak(558, game);
            // !TOO BIG.
            return ret_val;

        L3700:
            //ObjectHandler.newsta_(obj, 559, 0, 0, game.Player.Winner, game);
            // !DO TAKE.
            game.Objects.oflag2[obj - 1] |= ObjectFlags2.TCHBT;
            //scrupd_(game.Objects.ofval[obj - 1], game);
            game.Objects.ofval[obj - 1] = 0;

        L4000:
            ret_val = true;
            // !SUCCESS.
            return ret_val;
        }

        public static bool syneql_(int prep, int obj, int sprep, int sfl1, int sfl2, Game game)
        {
            bool ret_val;

            if (obj == 0)
            {
                goto L100;
            }
            // !ANY OBJECT?
            ret_val = prep == (sprep & (int)SyntaxObjectFlags.VPMASK) && (sfl1 & (int)game.Objects.oflag1[obj - 1] | sfl2 & (int)game.Objects.oflag2[obj - 1]) != 0;
            return ret_val;

        L100:
            ret_val = prep == 0 && sfl1 == 0 && sfl2 == 0;
            return ret_val;
        }

        /// <summary>
        /// unpack_ - Unpack syntax specification
        /// </summary>
        /// <param name="oldj"></param>
        /// <param name="j"></param>
        /// <param name="game"></param>
        private static void unpack_(int oldj, out int j, Game game)
        {
            int i;

            // !CLEAR SYNTAX.
            game.Syntax.dfl1 = 0;
            game.Syntax.dfl2 = 0;
            game.Syntax.dfw1 = 0;
            game.Syntax.dfw2 = 0;
            game.Syntax.dobj = 0;
            game.Syntax.ifl1 = 0;
            game.Syntax.ifl2 = 0;
            game.Syntax.ifw1 = 0;
            game.Syntax.ifw2 = 0;
            game.Syntax.iobj = 0;
            game.Syntax.vflag = 0;
            // L10:

            game.Syntax.vflag = (SyntaxFlags)ParserConstants.vvoc[oldj - 1];
            j = oldj + 1;
            if ((game.Syntax.vflag & SyntaxFlags.SDIR) == 0)
            {
                return;
            }
            game.Syntax.dfl1 = -1;
            // !ASSUME STD.
            game.Syntax.dfl2 = -1;
            if ((game.Syntax.vflag & SyntaxFlags.SSTD) == 0)
            {
                goto L100;
            }
            game.Syntax.dfw1 = -1;
            // !YES.
            game.Syntax.dfw2 = -1;
            game.Syntax.dobj = (int)SyntaxObjectFlags.VABIT + (int)SyntaxObjectFlags.VRBIT + (int)SyntaxObjectFlags.VFBIT;
            goto L200;

        L100:
            game.Syntax.dobj = ParserConstants.vvoc[j - 1];
            // !NOT STD.
            game.Syntax.dfw1 = ParserConstants.vvoc[j];
            game.Syntax.dfw2 = ParserConstants.vvoc[j + 1];
            j += 3;
            if ((game.Syntax.dobj & (int)SyntaxObjectFlags.VEBIT) == 0)
            {
                goto L200;
            }
            game.Syntax.dfl1 = game.Syntax.dfw1;
            // !YES.
            game.Syntax.dfl2 = game.Syntax.dfw2;

        L200:
            if ((game.Syntax.vflag & SyntaxFlags.SIND) == 0)
            {
                return;
            }
            game.Syntax.ifl1 = -1;
            // !ASSUME STD.
            game.Syntax.ifl2 = -1;
            game.Syntax.iobj = ParserConstants.vvoc[j - 1];
            game.Syntax.ifw1 = ParserConstants.vvoc[j];
            game.Syntax.ifw2 = ParserConstants.vvoc[j + 1];
            j += 3;

            if ((game.Syntax.iobj & (int)SyntaxObjectFlags.VEBIT) == 0)
            {
                return;
            }
            game.Syntax.ifl1 = game.Syntax.ifw1;
            // !YES.
            game.Syntax.ifl2 = game.Syntax.ifw2;
        }

        /// <summary>
        /// schlst_ - Search for object
        /// </summary>
        /// <param name="oidx"></param>
        /// <param name="aidx"></param>
        /// <param name="rm"></param>
        /// <param name="cn"></param>
        /// <param name="ad"></param>
        /// <param name="spcobj"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private static int schlst_(int oidx, int aidx, int rm, int cn, int ad, int spcobj, Game game)
        {
            // System generated locals
            int ret_val, i__1, i__2;

            // Local variables
            int i, j, x;

            ret_val = 0;
            // !NO RESULT.
            i__1 = game.Objects.Count;
            for (i = 1; i <= i__1; ++i)
            {
                // !SEARCH OBJECTS.
                if ((game.Objects.oflag1[i - 1] & ObjectFlags.VISIBT) == 0 || (rm == 0 ||
                    !ObjectHandler.qhere_(i, rm, game))
                    && (cn == 0 || game.Objects.ocan[i - 1] != cn)
                    && (ad == 0 || game.Objects.oadv[i - 1] != ad))
                {
                    goto L1000;
                }

                if (!thisit_(oidx, aidx, i, spcobj, game))
                {
                    goto L200;
                }
                if (ret_val != 0)
                {
                    goto L2000;
                }
                // !GOT ONE ALREADY?
                ret_val = i;
            // !NO.

            // IF OPEN OR TRANSPARENT, SEARCH THE OBJECT ITSELF.

            L200:
                if ((game.Objects.oflag1[i - 1] & ObjectFlags.TRANBT) == 0 && (
                    game.Objects.oflag2[i - 1] & ObjectFlags2.OPENBT) == 0)
                {
                    goto L1000;
                }

                // SEARCH IS CONDUCTED IN REVERSE.  ALL OBJECTS ARE CHECKED TO
                // SEE IF THEY ARE AT SOME LEVEL OF CONTAINMENT INSIDE OBJECT 'I'.
                // IF THEY ARE AT LEVEL 1, OR IF ALL LINKS IN THE CONTAINMENT
                // CHAIN ARE OPEN, VISIBLE, AND HAVE SEARCHME SET, THEY CAN QUALIFY

                // AS A POTENTIAL MATCH.

                i__2 = game.Objects.Count;
                for (j = 1; j <= i__2; ++j)
                {
                    // !SEARCH OBJECTS.
                    if ((game.Objects.oflag1[j - 1] & ObjectFlags.VISIBT) == 0 || !thisit_(oidx, aidx, j, spcobj, game))
                    {
                        goto L500;
                    }
                    x = game.Objects.ocan[j - 1];
                // !GET CONTAINER.
                L300:
                    if (x == i)
                    {
                        goto L400;
                    }
                    // !INSIDE TARGET?
                    if (x == 0)
                    {
                        goto L500;
                    }
                    // !INSIDE ANYTHING?
                    if ((game.Objects.oflag1[x - 1] & ObjectFlags.VISIBT) == 0 || (
                        game.Objects.oflag1[x - 1] & ObjectFlags.TRANBT) == 0 && (
                        game.Objects.oflag2[x - 1] & ObjectFlags2.OPENBT) == 0 || (
                        game.Objects.oflag2[x - 1] & ObjectFlags2.SCHBT) == 0)
                    {
                        goto L500;
                    }
                    x = game.Objects.ocan[x - 1];
                    // !GO ANOTHER LEVEL.
                    goto L300;

                L400:
                    if (ret_val != 0)
                    {
                        goto L2000;
                    }
                    // !ALREADY GOT ONE?
                    ret_val = j;
                // !NO.
                L500:
                    ;
                }

            L1000:
                ;
            }
            return ret_val;

        L2000:
            ret_val = -ret_val;
            // !AMB RETURN.
            return ret_val;
        }

        /// <summary>
        /// thisisit_ - Validate object vs description
        /// </summary>
        /// <param name="oidx"></param>
        /// <param name="aidx"></param>
        /// <param name="obj"></param>
        /// <param name="spcobj"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        private static bool thisit_(int oidx, int aidx, int obj, int spcobj, Game game)
        {
            //    THE FOLLOWING DATA STATEMENT USED RADIX-50 NOTATION (R50MIN/1RA/)

            //       IN RADIX-50 NOTATION, AN "A" IN THE FIRST POSITION IS
            //       ENCODED AS 1*40*40 = 1600.

            const int r50min = 1600;

            bool ret_val;
            int i;

            ret_val = false;
            // !ASSUME NO MATCH.
            if (spcobj != 0 && obj == spcobj)
            {
                goto L500;
            }

            // CHECK FOR OBJECT NAMES

            i = oidx + 1;
        L100:
            ++i;
            if (ParserConstants.ovoc[i - 1] <= 0 || ParserConstants.ovoc[i - 1] >= r50min)
            {
                return ret_val;
            }
            // !IF DONE, LOSE.
            if (ParserConstants.ovoc[i - 1] != obj)
            {
                goto L100;
            }
            // !IF FAIL, CONT.

            if (aidx == 0)
            {
                goto L500;
            }
            // !ANY ADJ?
            i = aidx + 1;
        L200:
            ++i;
            if (ParserConstants.avoc[i - 1] <= 0 || ParserConstants.avoc[i - 1] >= r50min)
            {
                return ret_val;
            }
            // !IF DONE, LOSE.
            if (ParserConstants.avoc[i - 1] != obj)
            {
                goto L200;
            }
        // !IF FAIL, CONT.

        L500:
            ret_val = true;
            return ret_val;
        }

        // THIS ROUTINE DETAILS ON BIT 2 OF PRSFLG
        private static int sparse_(int[] lbuf, int llnt, bool vbflag, Game game)
        {
            // 	DATA R50MIN/1RA/,R50WAL/3RWAL/
            const int r50min = 1600;
            const int r50wal = 36852;

            int ret_val, i__1, i__2;

            int i, j, adj;
            int obj;
            int prep, pptr, lbuf1 = 0, lbuf2 = 0;
            int buzlnt, prplnt, dirlnt;

            // Parameter adjustments
            //--lbuf;

            ret_val = -1;
            // !ASSUME PARSE FAILS.
            adj = 0;
            // !CLEAR PARTS HOLDERS.
            game.pv_1.act = 0;
            prep = 0;
            pptr = 0;
            game.objvec.o1 = 0;
            game.objvec.o2 = 0;
            game.prpvec.p1 = 0;
            game.prpvec.p2 = 0;

            buzlnt = 20;
            prplnt = 48;
            dirlnt = 75;
            // SPARSE, PAGE 8

            // NOW LOOP OVER INPUT BUFFER OF LEXICAL TOKENS.

            i__1 = llnt;
            for (i = 1; i <= i__1; i += 2)
            {
                // !TWO WORDS/TOKEN.
                lbuf1 = lbuf[i];
                // !GET CURRENT TOKEN.
                lbuf2 = lbuf[i + 1];

                if (lbuf1 == 0)
                {
                    goto L1500;
                }
                // !END OF BUFFER?

                // CHECK FOR BUZZ WORD

                i__2 = buzlnt;
                for (j = 1; j <= i__2; j += 2)
                {
                    if (lbuf1 == ParserConstants.bvoc[j - 1] && lbuf2 == ParserConstants.bvoc[j])
                    {
                        goto L1000;
                    }
                    // L50:
                }

                // CHECK FOR ACTION OR DIRECTION

                if (game.pv_1.act != 0)
                {
                    goto L75;
                }

                // !GOT ACTION ALREADY?
                j = 1;
            // !CHECK FOR ACTION.
            L125:
                if (lbuf1 == ParserConstants.vvoc[j - 1] && lbuf2 == ParserConstants.vvoc[j])
                {
                    goto L3000;
                }

                // L150:
                j += 2;

                // !ADV TO NEXT SYNONYM.
                if (!(ParserConstants.vvoc[j - 1] > 0 && ParserConstants.vvoc[j - 1] < r50min))
                {
                    goto L125;
                }

                // !ANOTHER VERB?
                j = j + ParserConstants.vvoc[j - 1] + 1;

                // !NO, ADVANCE OVER SYNTAX.
                if (ParserConstants.vvoc[j - 1] != -1)
                {
                    goto L125;
                }
            // !TABLE DONE?

            L75:
                if (game.pv_1.act != 0 && (ParserConstants.vvoc[game.pv_1.act - 1] != r50wal || prep != 0))
                {
                    goto L200;
                }

                i__2 = dirlnt;
                for (j = 1; j <= i__2; j += 3)
                {
                    // !THEN CHK FOR DIR.
                    if (lbuf1 == ParserConstants.dvoc[j - 1] && lbuf2 == ParserConstants.dvoc[j])
                    {
                        goto L2000;
                    }
                    // L100:
                }

            // NOT AN ACTION, CHECK FOR PREPOSITION, ADJECTIVE, OR OBJECT.

            L200:
                i__2 = prplnt;
                for (j = 1; j <= i__2; j += 3)
                {
                    // !LOOK FOR PREPOSITION.
                    if (lbuf1 == ParserConstants.pvoc[j - 1] && lbuf2 == ParserConstants.pvoc[j])
                    {
                        goto L4000;
                    }
                    // L250:
                }

                j = 1;
            // !LOOK FOR ADJECTIVE.
            L300:
                if (lbuf1 == ParserConstants.avoc[j - 1] && lbuf2 == ParserConstants.avoc[j])
                {
                    goto L5000;
                }

                ++j;
            L325:
                ++j;

                // !ADVANCE TO NEXT ENTRY.
                if (ParserConstants.avoc[j - 1] > 0 && ParserConstants.avoc[j - 1] < r50min)
                {
                    goto L325;
                }

                // !A RADIX 50 CONSTANT?
                if (ParserConstants.avoc[j - 1] != -1)
                {
                    goto L300;
                }

                // !POSSIBLY, END TABLE?
                j = 1;
            // !LOOK FOR OBJECT.
            L450:
                if (lbuf1 == ParserConstants.ovoc[j - 1] && lbuf2 == ParserConstants.ovoc[j])
                {
                    goto L600;
                }

                ++j;
            L500:
                ++j;

                if (ParserConstants.ovoc[j - 1] > 0 && ParserConstants.ovoc[j - 1] < r50min)
                {
                    goto L500;
                }

                if (ParserConstants.ovoc[j - 1] != -1)
                {
                    goto L450;
                }

                // NOT RECOGNIZABLE

                if (vbflag)
                {
                    MessageHandler.Speak(601, game);
                }

                return ret_val;
            // SPARSE, PAGE 9

            // OBJECT PROCESSING (CONTINUATION OF DO LOOP ON PREV PAGE)

            L600:
                obj = getobj_(j, adj, 0, game);

                // !IDENTIFY OBJECT.
                if (obj <= 0)
                {
                    goto L6000;
                }

                // !IF LE, COULDNT.
                if (obj != (int)ObjectIndices.itobj)
                {
                    goto L650;
                }

                // !"IT"?
                obj = getobj_(0, 0, game.Last.lastit, game);
                // !FIND LAST.
                if (obj <= 0)
                {
                    goto L6000;
                }
            // !IF LE, COULDNT.

            L650:
                if (prep == 9)
                {
                    goto L8000;
                }

                // !"OF" OBJ?
                if (pptr == 2)
                {
                    goto L7000;
                }

                // !TOO MANY OBJS?
                ++pptr;

                if (pptr == 1)
                {
                    game.objvec.o1 = obj;
                }
                else if (pptr == 2)
                {
                    game.objvec.o2 = obj;
                }
                else
                {
                    throw new InvalidOperationException();
                }

                // !STUFF INTO VECTOR.
                if (pptr == 1)
                {
                    game.prpvec.p1 = prep;
                }
                else if (pptr == 2)
                {
                    game.prpvec.p2 = prep;
                }
                else
                {
                    throw new InvalidOperationException();
                }

            L700:
                prep = 0;
                adj = 0;
                // Go to end of do loop (moved "1000 CONTINUE" to end of module, to avoid
                // complaints about people jumping back into the doloop.)
                goto L1000;
            // SPARSE, PAGE 10

            // SPECIAL PARSE PROCESSORS

            // 2000--	DIRECTION

            L2000:

                game.ParserVectors.prsa = (int)VIndices.walkw;
                game.ParserVectors.prso = ParserConstants.dvoc[j + 1];

                ret_val = 1;
                return ret_val;

            // 3000--	ACTION

            L3000:
                game.pv_1.act = j;
                game.Orphans.oact = 0;
                goto L1000;

            // 4000--	PREPOSITION

            L4000:
                if (prep != 0)
                {
                    goto L4500;
                }

                prep = ParserConstants.pvoc[j + 1];
                adj = 0;
                goto L1000;

            L4500:
                if (vbflag)
                {
                    MessageHandler.Speak(616, game);
                }

                return ret_val;

            // 5000--	ADJECTIVE

            L5000:
                adj = j;
                j = game.Orphans.oname & game.Orphans.oflag;
                if (j != 0 && i >= llnt)
                {
                    goto L600;
                }

                goto L1000;

            // 6000--	UNIDENTIFIABLE OBJECT (INDEX INTO OVOC IS J)

            L6000:
                if (obj < 0)
                {
                    goto L6100;
                }

                j = 579;

                if (RoomHandler.IsRoomLit(game.Player.Here, game))
                {
                    j = 618;
                }

                if (vbflag)
                {
                    MessageHandler.Speak(j, game);
                }

                return ret_val;

            L6100:
                if (obj != -10000)
                {
                    goto L6200;
                }

                if (vbflag)
                {
                    MessageHandler.rspsub_(620, game.Objects.odesc2[game.Adventurers.Vehicles[game.Player.Winner - 1] - 1], game);
                }

                return ret_val;

            L6200:
                if (vbflag)
                {
                    MessageHandler.Speak(619, game);
                }

                if (game.pv_1.act == 0)
                {
                    game.pv_1.act = game.Orphans.oflag & game.Orphans.oact;
                }

                Orphans.Orphan(-1, game.pv_1.act, game.objvec.o1, prep, j, game);
                return ret_val;

            // 7000--	TOO MANY OBJECTS.

            L7000:
                if (vbflag)
                {
                    MessageHandler.Speak(617, game);
                }

                return ret_val;

            // 8000--	RANDOMNESS FOR "OF" WORDS

            L8000:
                if ((pptr == 1 && game.objvec.o1 == obj) || (pptr == 2 && game.objvec.o2 == obj))
                {
                    {
                        goto L700;
                    }
                }

                if (vbflag)
                {
                    MessageHandler.Speak(601, game);
                }

                return ret_val;

            // End of do-loop.

            L1000:
                ;
            }
        // !AT LAST.

        // NOW SOME MISC CLEANUP -- We fell out of the do-loop

        L1500:
            if (game.pv_1.act == 0)
            {
                game.pv_1.act = game.Orphans.oflag & game.Orphans.oact;
            }

            if (game.pv_1.act == 0)
            {
                goto L9000;
            }

            // !IF STILL NONE, PUNT.
            if (adj != 0)
            {
                goto L10000;
            }

            // !IF DANGLING ADJ, PUNT.
            if (game.Orphans.oflag != 0 && game.Orphans.oprep != 0 && prep == 0 && game.objvec.o1 != 0
                && game.objvec.o2 == 0 && game.pv_1.act == game.Orphans.oact)
            {
                goto L11000;
            }

            ret_val = 0;
            // !PARSE SUCCEEDS.
            if (prep == 0)
            {
                goto L1750;
            }

            // !IF DANGLING PREP,
            if (pptr == 1 && game.prpvec.p1 != 0 || (pptr == 2 && game.prpvec.p2 != 0))
            {
                goto L12000;
            }

            if (pptr == 1)
            {
                game.prpvec.p1 = prep;
            }
            else
            {
                game.prpvec.p2 = prep;
            }

        // !CVT TO 'PICK UP FROB'.

        // 1750--	RETURN A RESULT

        L1750:
            // !WIN.
            return ret_val;
        // !LOSE.

        // 9000--	NO ACTION, PUNT

        L9000:
            if (game.objvec.o1 == 0)
            {
                goto L10000;
            }

            // !ANY DIRECT OBJECT?
            if (vbflag)
            {
                MessageHandler.rspsub_(621, game.Objects.odesc2[game.objvec.o1 - 1], game);
            }

            // !WHAT TO DO?
            Orphans.Orphan(-1, 0, game.objvec.o1, 0, 0, game);
            return ret_val;

        // 10000--	TOTAL CHOMP

        L10000:
            if (vbflag)
            {
                MessageHandler.Speak(622, game);
            }

            // !HUH?
            return ret_val;

        // 11000--	ORPHAN PREPOSITION.  CONDITIONS ARE
        // 		O1.NE.0, O2=0, PREP=0, ACT=OACT

        L11000:
            if (game.Orphans.oslot != 0)
            {
                goto L11500;
            }

            // !ORPHAN OBJECT?
            game.prpvec.p1 = game.Orphans.oprep;
            // !NO, JUST USE PREP.
            goto L1750;

        L11500:
            game.objvec.o2 = game.objvec.o1;

            // !YES, USE AS DIRECT OBJ.
            game.prpvec.p2 = game.Orphans.oprep;
            game.objvec.o1 = game.Orphans.oslot;
            game.prpvec.p1 = 0;
            goto L1750;

        // 12000--	TRUE HANGING PREPOSITION.
        // 		ORPHAN FOR LATER.

        L12000:
            Orphans.Orphan(-1, game.pv_1.act, 0, prep, 0, game);
            // !ORPHAN PREP.
            goto L1750;
        }

        // THIS ROUTINE DETAILS ON BIT 3 OF PRSFLG
        private static int getobj_(int oidx, int aidx, int spcobj, Game game)
        {
            int ret_val, i__1;

            int i, av;
            int obj;
            int nobj;
            bool chomp;

            chomp = false;
            av = game.Adventurers.Vehicles[game.Player.Winner - 1];
            obj = 0;

            // !ASSUME DARK.
            if (!RoomHandler.IsRoomLit(game.Player.Here, game))
            {
                goto L200;
            }
            // !LIT?

            obj = schlst_(oidx, aidx, game.Player.Here, 0, 0, spcobj, game);
            // !SEARCH ROOM.
            if (obj < 0)
            {
                goto L1000;
            }
            else if (obj == 0)
            {
                goto L200;
            }
            else
            {
                goto L100;
            }

        // !TEST RESULT.
        L100:
            if (av == 0 || av == obj || (game.Objects.oflag2[obj - 1] & ObjectFlags2.FINDBT)
                != 0)
            {
                goto L200;
            }
            if (game.Objects.ocan[obj - 1] == av)
            {
                goto L200;
            }
            // !TEST IF REACHABLE.
            chomp = true;
        // !PROBABLY NOT.

        L200:
            if (av == 0)
            {
                goto L400;
            }

            // !IN VEHICLE?
            nobj = schlst_(oidx, aidx, 0, av, 0, spcobj, game);

            // !SEARCH VEHICLE.
            if (nobj < 0)
            {
                goto L1100;
            }
            else if (nobj == 0)
            {
                goto L400;
            }
            else
            {
                goto L300;
            }

        // !TEST RESULT.
        L300:
            chomp = false;
            // !REACHABLE.
            if (obj == nobj)
            {
                goto L400;
            }

            // !SAME AS BEFORE?
            if (obj != 0)
            {
                nobj = -nobj;
            }
            // !AMB RESULT?
            obj = nobj;

        L400:
            nobj = schlst_(oidx, aidx, 0, 0, game.Player.Winner, spcobj, game);
            // !SEARCH ADVENTURER.
            if (nobj < 0)
            {
                goto L1100;
            }
            else if (nobj == 0)
            {
                goto L600;
            }
            else
            {
                goto L500;
            }

        // !TEST RESULT
        L500:
            if (obj != 0)
            {
                nobj = -nobj;
            }
        // !AMB RESULT?
        L1100:
            obj = nobj;
        // !RETURN NEW OBJECT.
        L600:
            if (chomp)
            {
                obj = -10000;
            }
        // !UNREACHABLE.
        L1000:
            ret_val = obj;

            if (ret_val != 0)
            {
                goto L1500;
            }
            // !GOT SOMETHING?
            i__1 = game.Objects.Count;
            for (i = game.Star.strbit + 1; i <= i__1; ++i)
            {
                // !NO, SEARCH GLOBALS.
                if (!thisit_(oidx, aidx, i, spcobj, game))
                {
                    goto L1200;
                }

                if (!GlobalHandler.ghere_(i, game.Player.Here, game))
                {
                    goto L1200;
                }
                // !CAN IT BE HERE?
                if (ret_val != 0)
                {
                    ret_val = -i;
                }
                // !AMB MATCH?
                if (ret_val == 0)
                {
                    ret_val = i;
                }
            L1200:
                ;
            }

        L1500:
            // !END OF SEARCH.
            return ret_val;
        }

        public static bool vappli_(string input, int ri, Game game)
        {
            const int mxnop = 39;
            const int mxsmp = 99;

            int i__1;
            bool ret_val;

            int melee;
            bool f;
            int i, j, av;
            int rmk;
            int odi2 = 0, odo2 = 0;

            ret_val = true;
            /* 						!ASSUME WINS. */

            if (game.ParserVectors.prso > 220)
            {
                goto L5;
            }

            if (game.ParserVectors.prso != 0)
            {
                odo2 = game.Objects.odesc2[game.ParserVectors.prso - 1];
            }
        /* 						!SET UP DESCRIPTORS. */
        L5:
            if (game.ParserVectors.prsi != 0)
            {
                odi2 = game.Objects.odesc2[game.ParserVectors.prsi - 1];
            }
            av = game.Adventurers.Vehicles[game.Player.Winner - 1];
            rmk = game.rnd_(6) + 372;
            /* 						!REMARK FOR HACK-HACKS. */

            if (ri == 0)
            {
                goto L10;
            }
            /* 						!ZERO IS FALSE. */
            if (ri <= mxnop)
            {
                return ret_val;
            }
            /* 						!NOP? */
            if (ri <= mxsmp)
            {
                goto L100;
            }
            /* 						!SIMPLE VERB? */
            switch (ri - mxsmp)
            {
                case 1: goto L18000;
                case 2: goto L20000;
                case 3: goto L22000;
                case 4: goto L23000;
                case 5: goto L24000;
                case 6: goto L25000;
                case 7: goto L26000;
                case 8: goto L27000;
                case 9: goto L28000;
                case 10: goto L29000;
                case 11: goto L30000;
                case 12: goto L31000;
                case 13: goto L32000;
                case 14: goto L33000;
                case 15: goto L34000;
                case 16: goto L35000;
                case 17: goto L36000;
                case 18: goto L38000;
                case 19: goto L39000;
                case 20: goto L40000;
                case 21: goto L41000;
                case 22: goto L42000;
                case 23: goto L43000;
                case 24: goto L44000;
                case 25: goto L45000;
                case 26: goto L46000;
                case 27: goto L47000;
                case 28: goto L48000;
                case 29: goto L49000;
                case 30: goto L50000;
                case 31: goto L51000;
                case 32: goto L52000;
                case 33: goto L53000;
                case 34: goto L55000;
                case 35: goto L56000;
                case 36: goto L58000;
                case 37: goto L59000;
                case 38: goto L60000;
                case 39: goto L63000;
                case 40: goto L64000;
                case 41: goto L65000;
                case 42: goto L66000;
                case 43: goto L68000;
                case 44: goto L69000;
                case 45: goto L70000;
                case 46: goto L71000;
                case 47: goto L72000;
                case 48: goto L73000;
                case 49: goto L74000;
                case 50: goto L77000;
                case 51: goto L78000;
                case 52: goto L80000;
                case 53: goto L81000;
                case 54: goto L82000;
                case 55: goto L83000;
                case 56: goto L84000;
                case 57: goto L85000;
                case 58: goto L86000;
                case 59: goto L87000;
                case 60: goto L88000;
            }

            throw new InvalidOperationException("7");

        /* ALL VERB PROCESSORS RETURN HERE TO DECLARE FAILURE. */

        L10:
            ret_val = false;
            /* 						!LOSE. */
            return ret_val;

        /* SIMPLE VERBS ARE HANDLED EXTERNALLY. */

        L100:
            ret_val = sverbs_(game, input, ri);
            return ret_val;
        /* VAPPLI, PAGE 3 */

        /* V100--	READ.  OUR FIRST REAL VERB. */

        L18000:
            if (RoomHandler.IsRoomLit(game.Player.Here, game))
            {
                goto L18100;
            }
            /* 						!ROOM LIT? */
            MessageHandler.Speak(356, game);
            /* 						!NO, CANT READ. */
            return ret_val;

        L18100:
            if (game.ParserVectors.prsi == 0)
            {
                goto L18200;
            }

            /* 						!READ THROUGH OBJ? */
            if ((game.Objects.oflag1[game.ParserVectors.prsi - 1] & ObjectFlags.TRANBT) != 0)
            {
                goto L18200;
            }

            MessageHandler.rspsub_(357, odi2, game);
            /* 						!NOT TRANSPARENT. */
            return ret_val;

        L18200:
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.READBT) != 0)
            {
                goto L18300;
            }
            MessageHandler.rspsub_(358, odo2, game);
            /* 						!NOT READABLE. */
            return ret_val;

        L18300:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.Speak(game.Objects.oread[game.ParserVectors.prso - 1], game);
            }
            return ret_val;

        /* V101--	MELT.  UNLESS OBJECT HANDLES, JOKE. */

        L20000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.rspsub_(361, odo2, game);
            }
            return ret_val;

        /* V102--	INFLATE.  WORKS ONLY WITH BOATS. */

        L22000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.Speak(368, game);
            }
            /* 						!OBJ HANDLE? */
            return ret_val;

        /* V103--	DEFLATE. */

        L23000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.Speak(369, game);
            }
            /* 						!OBJ HANDLE? */
            return ret_val;
        /* VAPPLI, PAGE 4 */

        /* V104--	ALARM.  IF SLEEPING, WAKE HIM UP. */

        L24000:
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.SLEPBT) == 0)
            {
                goto L24100;
            }
            ret_val = ObjectHandler.objact_(game);
            /* 						!SLEEPING, LET OBJ DO. */
            return ret_val;

        L24100:
            MessageHandler.rspsub_(370, odo2, game);
            /* 						!JOKE. */
            return ret_val;

        /* V105--	EXORCISE.  OBJECTS HANDLE. */

        L25000:
            f = ObjectHandler.objact_(game);
            /* 						!OBJECTS HANDLE. */
            return ret_val;

        /* V106--	PLUG.  LET OBJECTS HANDLE. */

        L26000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.Speak(371, game);
            }
            return ret_val;

        /* V107--	KICK.  IF OBJECT IGNORES, JOKE. */

        L27000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.rspsb2_(378, odo2, rmk, game);
            }
            return ret_val;

        /* V108--	WAVE.  SAME. */

        L28000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.rspsb2_(379, odo2, rmk, game);
            }
            return ret_val;

        /* V109,V110--	RAISE, LOWER.  SAME. */

        L29000:
        L30000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.rspsb2_(380, odo2, rmk, game);
            }
            return ret_val;

        /* V111--	RUB.  SAME. */

        L31000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.rspsb2_(381, odo2, rmk, game);
            }
            return ret_val;

        /* V112--	PUSH.  SAME. */

        L32000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.rspsb2_(382, odo2, rmk, game);
            }
            return ret_val;
        /* VAPPLI, PAGE 5 */

        /* V113--	UNTIE.  IF OBJECT IGNORES, JOKE. */

        L33000:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }

            /* 						!OBJECT HANDLE? */
            i = 383;
            /* 						!NO, NOT TIED. */
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.TIEBT) == 0)
            {
                i = 384;
            }

            MessageHandler.Speak(i, game);
            return ret_val;

        /* V114--	TIE.  NEVER REALLY WORKS. */

        L34000:
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.TIEBT) != 0)
            {
                goto L34100;
            }
            MessageHandler.Speak(385, game);
            /* 						!NOT TIEABLE. */
            return ret_val;

        L34100:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.rspsub_(386, odo2, game);
            }
            /* 						!JOKE. */
            return ret_val;

        /* V115--	TIE UP.  NEVER REALLY WORKS. */

        L35000:
            if ((game.Objects.oflag2[game.ParserVectors.prsi - 1] & ObjectFlags2.TIEBT) != 0)
            {
                goto L35100;
            }

            MessageHandler.rspsub_(387, odo2, game);
            /* 						!NOT TIEABLE. */
            return ret_val;

        L35100:
            i = 388;

            /* 						!ASSUME VILLAIN. */
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.VILLBT) == 0)
            {
                i = 389;
            }

            MessageHandler.rspsub_(i, odo2, game);
            /* 						!JOKE. */
            return ret_val;

        /* V116--	TURN.  OBJECT MUST HANDLE. */

        L36000:
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.TURNBT) != 0)
            {
                goto L36100;
            }

            MessageHandler.Speak(390, game);
            /* 						!NOT TURNABLE. */
            return ret_val;

        L36100:
            if ((game.Objects.oflag1[game.ParserVectors.prsi - 1] & ObjectFlags.TOOLBT) != 0)
            {
                goto L36200;
            }

            MessageHandler.rspsub_(391, odi2, game);
            /* 						!NOT A TOOL. */
            return ret_val;

        L36200:
            ret_val = ObjectHandler.objact_(game);
            /* 						!LET OBJECT HANDLE. */
            return ret_val;

        /* V117--	BREATHE.  BECOMES INFLATE WITH LUNGS. */

        L38000:
            game.ParserVectors.prsa = (int)VIndices.inflaw;
            game.ParserVectors.prsi = (int)ObjectIndices.lungs;
            goto L22000;
        /* 						!HANDLE LIKE INFLATE. */

        /* V118--	KNOCK.  MOSTLY JOKE. */

        L39000:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            /* 						!OBJ HANDLE? */
            i = 394;
            /* 						!JOKE FOR DOOR. */
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.DOORBT) == 0)
            {
                i = 395;
            }
            MessageHandler.rspsub_(i, odo2, game);
            /* 						!JOKE FOR NONDOORS TOO. */
            return ret_val;

        /* V119--	LOOK. */

        L40000:
            if (game.ParserVectors.prso != 0)
            {
                goto L41500;
            }
            /* 						!SOMETHING TO LOOK AT? */
            ret_val = RoomHandler.RoomDescription(3, game);
            /* 						!HANDLED BY RMDESC. */
            return ret_val;

        /* V120--	EXAMINE. */

        L41000:
            if (game.ParserVectors.prso != 0)
            {
                goto L41500;
            }
            /* 						!SOMETHING TO EXAMINE? */
            ret_val = RoomHandler.RoomDescription(0, game);
            /* 						!HANDLED BY RMDESC. */
            return ret_val;

        L41500:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            /* 						!OBJ HANDLE? */
            i = game.Objects.oread[game.ParserVectors.prso - 1];
            /* 						!GET READING MATERIAL. */
            if (i != 0)
            {
                MessageHandler.Speak(i, game);
            }

            /* 						!OUTPUT IF THERE, */
            if (i == 0)
            {
                MessageHandler.rspsub_(429, odo2, game);
            }

            /* 						!OTHERWISE DEFAULT. */
            game.ParserVectors.prsa = (int)VIndices.foow;
            /* 						!DEFUSE ROOM PROCESSORS. */
            return ret_val;

        /* V121--	SHAKE.  IF HOLLOW OBJECT, SOME ACTION. */

        L42000:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            /* 						!OBJECT HANDLE? */
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.VILLBT) == 0)
            {
                goto L42100;
            }

            MessageHandler.Speak(371, game);

            /* 						!JOKE FOR VILLAINS. */
            return ret_val;

        L42100:
            if (ObjectHandler.IsObjectEmpty(game.ParserVectors.prso, game) || (game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.TAKEBT) == 0)
            {
                goto L10;
            }
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.OPENBT) != 0)
            {
                goto L42300;
            }
            /* 						!OPEN?  SPILL. */
            MessageHandler.rspsub_(396, odo2, game);
            /* 						!NO, DESCRIBE NOISE. */
            return ret_val;

        L42300:
            MessageHandler.rspsub_(397, odo2, game);
            /* 						!SPILL THE WORKS. */
            i__1 = game.Objects.Count;
            for (i = 1; i <= i__1; ++i)
            {
                /* 						!SPILL CONTENTS. */
                if (game.Objects.ocan[i - 1] != game.ParserVectors.prso)
                {
                    goto L42500;
                }

                /* 						!INSIDE? */
                game.Objects.oflag2[i - 1] |= ObjectFlags2.TCHBT;
                if (av == 0)
                {
                    goto L42400;
                }

                /* 						!IN VEHICLE? */
                ObjectHandler.newsta_(i, 0, 0, av, 0, game);
                /* 						!YES, SPILL IN THERE. */
                goto L42500;

            L42400:
                ObjectHandler.newsta_(i, 0, game.Player.Here, 0, 0, game);
                /* 						!NO, SPILL ON FLOOR, */
                if (i == (int)ObjectIndices.water)
                {
                    ObjectHandler.newsta_(i, 133, 0, 0, 0, game);
                }
            /* 						!BUT WATER DISAPPEARS. */
            L42500:
                ;
            }
            return ret_val;

        /* V122--	MOVE.  MOSTLY JOKES. */

        L43000:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            /* 						!OBJ HANDLE? */
            i = 398;
            /* 						!ASSUME NOT HERE. */
            if (ObjectHandler.qhere_(game.ParserVectors.prso, game.Player.Here, game))
            {
                i = 399;
            }
            MessageHandler.rspsub_(i, odo2, game);
            /* 						!JOKE. */
            return ret_val;
        /* VAPPLI, PAGE 6 */

        /* V123--	TURN ON. */

        L44000:
            f = RoomHandler.IsRoomLit(game.Player.Here, game);
            /* 						!RECORD IF LIT. */
            if (ObjectHandler.objact_(game))
            {
                goto L44300;
            }
            /* 						!OBJ HANDLE? */
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.LITEBT) != 0
                && game.Objects.oadv[game.ParserVectors.prso - 1] == game.Player.Winner)
            {
                goto L44100;
            }
            MessageHandler.Speak(400, game);
            /* 						!CANT DO IT. */
            return ret_val;

        L44100:
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.ONBT) == 0)
            {
                goto L44200;
            }
            MessageHandler.Speak(401, game);
            /* 						!ALREADY ON. */
            return ret_val;

        L44200:
            game.Objects.oflag1[game.ParserVectors.prso - 1] |= ObjectFlags.ONBT;
            MessageHandler.rspsub_(404, odo2, game);

        L44300:
            if (!f && RoomHandler.IsRoomLit(game.Player.Here, game))
            {
                f = RoomHandler.RoomDescription(0, game);
            }
            /* 						!ROOM NEWLY LIT. */
            return ret_val;

        /* V124--	TURN OFF. */

        L45000:
            if (ObjectHandler.objact_(game))
            {
                goto L45300;
            }
            /* 						!OBJ HANDLE? */
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.LITEBT) != 0 &&
                game.Objects.oadv[game.ParserVectors.prso - 1] == game.Player.Winner)
            {
                goto L45100;
            }
            MessageHandler.Speak(402, game);
            /* 						!CANT DO IT. */
            return ret_val;

        L45100:
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.ONBT) != 0)
            {
                goto L45200;
            }
            MessageHandler.Speak(403, game);
            /* 						!ALREADY OFF. */
            return ret_val;

        L45200:
            game.Objects.oflag1[game.ParserVectors.prso - 1] &= ~ObjectFlags.ONBT;
            MessageHandler.rspsub_(405, odo2, game);
        L45300:
            if (!RoomHandler.IsRoomLit(game.Player.Here, game))
            {
                MessageHandler.Speak(406, game);
            }
            /* 						!MAY BE DARK. */
            return ret_val;

        /* V125--	OPEN.  A FINE MESS. */

        L46000:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            /* 						!OBJ HANDLE? */
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.CONTBT) != 0)
            {
                goto L46100;
            }
        L46050:
            MessageHandler.rspsub_(407, odo2, game);
            /* 						!NOT OPENABLE. */
            return ret_val;

        L46100:
            if (game.Objects.ocapac[game.ParserVectors.prso - 1] != 0)
            {
                goto L46200;
            }
            MessageHandler.rspsub_(408, odo2, game);
            /* 						!NOT OPENABLE. */
            return ret_val;

        L46200:
            if (!((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.OPENBT) != 0))
            {
                goto L46225;
            }
            MessageHandler.Speak(412, game);
            /* 						!ALREADY OPEN. */
            return ret_val;

        L46225:
            game.Objects.oflag2[game.ParserVectors.prso - 1] |= ObjectFlags2.OPENBT;
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.TRANBT) != 0 || ObjectHandler.IsObjectEmpty(game.ParserVectors.prso, game))
            {
                goto L46300;
            }

            ObjectHandler.PrintDescription(game.ParserVectors.prso, 410, game);
            /* 						!PRINT CONTENTS. */
            return ret_val;

        L46300:
            MessageHandler.Speak(409, game);
            /* 						!DONE */
            return ret_val;

        /* V126--	CLOSE. */

        L47000:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }

            /* 						!OBJ HANDLE? */
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.CONTBT) == 0)
            {
                goto L46050;
            }

            if (game.Objects.ocapac[game.ParserVectors.prso - 1] != 0)
            {
                goto L47100;
            }

            MessageHandler.rspsub_(411, odo2, game);
            /* 						!NOT CLOSABLE. */
            return ret_val;

        L47100:
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.OPENBT) != 0)
            {
                goto L47200;
            }

            /* 						!OPEN? */
            MessageHandler.Speak(413, game);
            /* 						!NO, JOKE. */
            return ret_val;

        L47200:
            game.Objects.oflag2[game.ParserVectors.prso - 1] &= ~ObjectFlags2.OPENBT;
            MessageHandler.Speak(414, game);
            /* 						!DONE. */
            return ret_val;
        /* VAPPLI, PAGE 7 */

        /* V127--	FIND.  BIG MEGILLA. */

        L48000:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            /* 						!OBJ HANDLE? */
            i = 415;
            /* 						!DEFAULT CASE. */
            if (ObjectHandler.qhere_(game.ParserVectors.prso, game.Player.Here, game))
            {
                goto L48300;
            }
            /* 						!IN ROOM? */
            if (game.Objects.oadv[game.ParserVectors.prso - 1] == game.Player.Winner)
            {
                goto L48200;
            }
            /* 						!ON WINNER? */
            j = game.Objects.ocan[game.ParserVectors.prso - 1];
            /* 						!DOWN ONE LEVEL. */
            if (j == 0)
            {
                goto L10;
            }
            if ((game.Objects.oflag1[j - 1] & ObjectFlags.TRANBT) == 0 && (!((
                game.Objects.oflag2[j - 1] & ObjectFlags2.OPENBT) != 0) || (
                game.Objects.oflag1[j - 1] & (int)ObjectFlags.DOORBT + ObjectFlags.CONTBT) == 0))
            {
                goto L10;
            }
            i = 417;
            /* 						!ASSUME IN ROOM. */
            if (ObjectHandler.qhere_(j, game.Player.Here, game))
            {
                goto L48100;
            }
            if (game.Objects.oadv[j - 1] != game.Player.Winner)
            {
                goto L10;
            }
            /* 						!NOT HERE OR ON PERSON. */
            i = 418;
        L48100:
            MessageHandler.rspsub_(i, game.Objects.odesc2[j - 1], game);
            /* 						!DESCRIBE FINDINGS. */
            return ret_val;

        L48200:
            i = 416;
        L48300:
            MessageHandler.rspsub_(i, odo2, game);
            /* 						!DESCRIBE FINDINGS. */
            return ret_val;

        /* V128--	WAIT.  RUN CLOCK DEMON. */

        L49000:
            MessageHandler.Speak(419, game);
            /* 						!TIME PASSES. */
            for (i = 1; i <= 3; ++i)
            {
                if (ClockEvents.clockd_(game))
                {
                    return ret_val;
                }
                /* L49100: */
            }
            return ret_val;

        /* V129--	SPIN. */
        /* V159--	TURN TO. */

        L50000:
        L88000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.Speak(663, game);
            }
            /* 						!IF NOT OBJ, JOKE. */
            return ret_val;

        /* V130--	BOARD.  WORKS WITH VEHICLES. */

        L51000:
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.VEHBT) != 0)
            {
                goto L51100;
            }
            MessageHandler.rspsub_(421, odo2, game);
            /* 						!NOT VEHICLE, JOKE. */
            return ret_val;

        L51100:
            if (ObjectHandler.qhere_(game.ParserVectors.prso, game.Player.Here, game))
            {
                goto L51200;
            }

            /* 						!HERE? */
            MessageHandler.rspsub_(420, odo2, game);
            /* 						!NO, JOKE. */
            return ret_val;

        L51200:
            if (av == 0)
            {
                goto L51300;
            }

            /* 						!ALREADY GOT ONE? */
            MessageHandler.rspsub_(422, odo2, game);
            /* 						!YES, JOKE. */
            return ret_val;

        L51300:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }

            /* 						!OBJ HANDLE? */
            MessageHandler.rspsub_(423, odo2, game);
            /* 						!DESCRIBE. */

            game.Adventurers.Vehicles[game.Player.Winner - 1] = game.ParserVectors.prso;

            if (game.Player.Winner != (int)AIndices.player)
            {
                game.Objects.ocan[game.Adventurers.Objects[game.Player.Winner - 1] - 1] = game.ParserVectors.prso;
            }
            return ret_val;

        /* V131--	DISEMBARK. */

        L52000:
            if (av == game.ParserVectors.prso)
            {
                goto L52100;
            }
            /* 						!FROM VEHICLE? */
            MessageHandler.Speak(424, game);
            /* 						!NO, JOKE. */
            return ret_val;

        L52100:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            /* 						!OBJ HANDLE? */
            if ((game.Rooms.RoomFlags[game.Player.Here - 1] & RoomFlags.RLAND) != 0)
            {
                goto L52200;
            }
            MessageHandler.Speak(425, game);
            /* 						!NOT ON LAND. */
            return ret_val;

        L52200:
            game.Adventurers.Vehicles[game.Player.Winner - 1] = 0;
            MessageHandler.Speak(426, game);
            if (game.Player.Winner != (int)AIndices.player)
            {
                ObjectHandler.newsta_(game.Adventurers.Objects[game.Player.Winner - 1], 0, game.Player.Here, 0, 0, game);
            }
            return ret_val;

        /* V132--	TAKE.  HANDLED EXTERNALLY. */

        L53000:
            ret_val = dverb1.take_(game, true);
            return ret_val;

        /* V133--	INVENTORY.  PROCESSED EXTERNALLY. */

        L55000:
            AdventurerHandler.PrintContents(game.Player.Winner, game);
            return ret_val;
        /* VAPPLI, PAGE 8 */

        /* V134--	FILL.  STRANGE DOINGS WITH WATER. */

        L56000:
            if (game.ParserVectors.prsi != 0)
            {
                goto L56050;
            }

            /* 						!ANY OBJ SPECIFIED? */
            if ((game.Rooms.RoomFlags[game.Player.Here - 1] & (int)RoomFlags.RWATER + RoomFlags.RFILL) != 0)
            {
                goto L56025;
            }

            MessageHandler.Speak(516, game);

            /* 						!NOTHING TO FILL WITH. */
            game.ParserVectors.prswon = false;
            /* 						!YOU LOSE. */
            return ret_val;

        L56025:
            game.ParserVectors.prsi = (int)ObjectIndices.gwate;

        /* 						!USE GLOBAL WATER. */
        L56050:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            /* 						!OBJ HANDLE? */
            if (game.ParserVectors.prsi != (int)ObjectIndices.gwate && game.ParserVectors.prsi != (int)ObjectIndices.water)
            {
                MessageHandler.rspsb2_(444, odi2, odo2, game);
            }
            return ret_val;

        /* V135,V136--	EAT/DRINK */

        L58000:
        L59000:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }

            /* 						!OBJ HANDLE? */
            if (game.ParserVectors.prso == (int)ObjectIndices.gwate)
            {
                goto L59500;
            }

            /* 						!DRINK GLOBAL WATER? */
            if (!((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.FOODBT) != 0))
            {
                goto L59400;
            }

            /* 						!EDIBLE? */
            if (game.Objects.oadv[game.ParserVectors.prso - 1] == game.Player.Winner)
            {
                goto L59200;
            }

        /* 						!YES, ON WINNER? */
        L59100:
            MessageHandler.rspsub_(454, odo2, game);
            /* 						!NOT ACCESSIBLE. */
            return ret_val;

        L59200:
            if (game.ParserVectors.prsa == (int)VIndices.drinkw)
            {
                goto L59300;
            }

            /* 						!DRINK FOOD? */
            ObjectHandler.newsta_(game.ParserVectors.prso, 455, 0, 0, 0, game);
            /* 						!NO, IT DISAPPEARS. */
            return ret_val;

        L59300:
            MessageHandler.Speak(456, game);
            /* 						!YES, JOKE. */
            return ret_val;

        L59400:
            if (!((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.DRNKBT) != 0))
            {
                goto L59600;
            }
            /* 						!DRINKABLE? */
            if (game.Objects.ocan[game.ParserVectors.prso - 1] == 0)
            {
                goto L59100;
            }
            /* 						!YES, IN SOMETHING? */
            if (game.Objects.oadv[game.Objects.ocan[game.ParserVectors.prso - 1] - 1] != game.Player.Winner)
            {
                goto L59100;
            }
            if ((game.Objects.oflag2[game.Objects.ocan[game.ParserVectors.prso - 1] - 1] & ObjectFlags2.OPENBT) != 0)
            {
                goto L59500;
            }
            /* 						!CONT OPEN? */
            MessageHandler.Speak(457, game);
            /* 						!NO, JOKE. */
            return ret_val;

        L59500:
            ObjectHandler.newsta_(game.ParserVectors.prso, 458, 0, 0, 0, game);
            /* 						!GONE. */
            return ret_val;

        L59600:
            MessageHandler.rspsub_(453, odo2, game);
            /* 						!NOT FOOD OR DRINK. */
            return ret_val;

        /* V137--	BURN.  COMPLICATED. */

        L60000:
            if (((int)game.Objects.oflag1[game.ParserVectors.prsi - 1] & (int)ObjectFlags.FLAMBT + (int)ObjectFlags.LITEBT + (int)ObjectFlags.ONBT) != ((int)ObjectFlags.FLAMBT + (int)ObjectFlags.LITEBT + (int)ObjectFlags.ONBT))
            {
                goto L60400;
            }

            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            /* 						!OBJ HANDLE? */
            if (game.Objects.ocan[game.ParserVectors.prso - 1] != (int)ObjectIndices.recep)
            {
                goto L60050;
            }
            /* 						!BALLOON? */
            if (ObjectHandler.oappli_(game.Objects.oactio[(int)ObjectIndices.ballo - 1], 0, game))
            {
                return ret_val;
            }
        /* 						!DID IT HANDLE? */
        L60050:
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.BURNBT) == 0)
            {
                goto L60300;
            }
            if (game.Objects.oadv[game.ParserVectors.prso - 1] != game.Player.Winner)
            {
                goto L60100;
            }
            /* 						!CARRYING IT? */
            MessageHandler.rspsub_(459, odo2, game);
            AdventurerHandler.jigsup_(game, 460);

            return ret_val;

        L60100:
            j = game.Objects.ocan[game.ParserVectors.prso - 1];
            /* 						!GET CONTAINER. */
            if (ObjectHandler.qhere_(game.ParserVectors.prso, game.Player.Here, game)
                || av != 0
                && j == av)
            {
                goto L60200;
            }
            if (j == 0)
            {
                goto L60150;
            }
            /* 						!INSIDE? */
            if (!((game.Objects.oflag2[j - 1] & ObjectFlags2.OPENBT) != 0))
            {
                goto L60150;
            }
            /* 						!OPEN? */
            if (ObjectHandler.qhere_(j, game.Player.Here, game)
                || av != 0
                && game.Objects.ocan[j - 1] == av)
            {
                goto L60200;
            }
        L60150:
            MessageHandler.Speak(461, game);
            /* 						!CANT REACH IT. */
            return ret_val;

        L60200:
            MessageHandler.rspsub_(462, odo2, game);
            /* 						!BURN IT. */
            ObjectHandler.newsta_(game.ParserVectors.prso, 0, 0, 0, 0, game);
            return ret_val;

        L60300:
            MessageHandler.rspsub_(463, odo2, game);
            /* 						!CANT BURN IT. */
            return ret_val;

        L60400:
            MessageHandler.rspsub_(301, odi2, game);
            /* 						!CANT BURN IT WITH THAT. */
            return ret_val;
        /* VAPPLI, PAGE 9 */

        /* V138--	MUNG.  GO TO COMMON ATTACK CODE. */

        L63000:
            i = 466;
            /* 						!CHOOSE PHRASE. */
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.VILLBT) != 0)
            {
                goto L66100;
            }

            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.rspsb2_(466, odo2, rmk, game);
            }

            return ret_val;

        /* V139--	KILL.  GO TO COMMON ATTACK CODE. */

        L64000:
            i = 467;
            /* 						!CHOOSE PHRASE. */
            goto L66100;

        /* V140--	SWING.  INVERT OBJECTS, FALL THRU TO ATTACK. */

        L65000:
            j = game.ParserVectors.prso;
            /* 						!INVERT. */
            game.ParserVectors.prso = game.ParserVectors.prsi;
            game.ParserVectors.prsi = j;
            j = odo2;
            odo2 = odi2;
            odi2 = j;
            game.ParserVectors.prsa = (int)VIndices.attacw;
        /* 						!FOR OBJACT. */

        /* V141--	ATTACK.  FALL THRU TO ATTACK CODE. */

        L66000:
            i = 468;

        /* COMMON MUNG/ATTACK/SWING/KILL CODE. */

        L66100:
            if (game.ParserVectors.prso != 0)
            {
                goto L66200;
            }

            /* 						!ANYTHING? */
            MessageHandler.Speak(469, game);
            /* 						!NO, JOKE. */
            return ret_val;

        L66200:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            /* 						!OBJ HANDLE? */
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.VILLBT) != 0)
            {
                goto L66300;
            }

            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.VICTBT) == 0)
            {
                MessageHandler.rspsub_(470, odo2, game);
            }

            return ret_val;

        L66300:
            j = 471;
            /* 						!ASSUME NO WEAPON. */
            if (game.ParserVectors.prsi == 0)
            {
                goto L66500;
            }

            if ((game.Objects.oflag2[game.ParserVectors.prsi - 1] & ObjectFlags2.WEAPBT) == 0)
            {
                goto L66400;
            }

            melee = 1;

            /* 						!ASSUME SWORD. */
            if (game.ParserVectors.prsi != (int)ObjectIndices.sword)
            {
                melee = 2;
            }

            /* 						!MUST BE KNIFE. */
            i = DemonHandler.blow_(game, (int)AIndices.player, game.ParserVectors.prso, melee, true, 0);
            /* 						!STRIKE BLOW. */
            return ret_val;

        L66400:
            j = 472;
        /* 						!NOT A WEAPON. */
        L66500:
            MessageHandler.rspsb2_(i, odo2, j, game);
            /* 						!JOKE. */
            return ret_val;
        /* VAPPLI, PAGE 10 */

        /* V142--	WALK.  PROCESSED EXTERNALLY. */

        L68000:
            ret_val = dverb2.walk_(game);
            return ret_val;

        /* V143--	TELL.  PROCESSED IN GAME. */

        L69000:
            MessageHandler.Speak(603, game);
            return ret_val;

        /* V144--	PUT.  PROCESSED EXTERNALLY. */

        L70000:
            ret_val = dverb1.put_(game, true);
            return ret_val;

        /* V145,V146,V147,V148--	DROP/GIVE/POUR/THROW */

        L71000:
        L72000:
        L73000:
        L74000:
            ret_val = dverb1.drop_(game, false);
            return ret_val;

        /* V149--	SAVE */

        L77000:
            if ((game.Rooms.RoomFlags[(int)RoomIndices.tstrs - 1] & RoomFlags.RSEEN) == 0)
            {
                goto L77100;
            }

            MessageHandler.Speak(828, game);
            /* 						!NO SAVES IN ENDGAME. */
            return ret_val;

        L77100:
            //savegm_();
            return ret_val;

        /* V150--	RESTORE */

        L78000:
            if ((game.Rooms.RoomFlags[(int)RoomIndices.tstrs - 1] & RoomFlags.RSEEN) == 0)
            {
                goto L78100;
            }

            MessageHandler.Speak(829, game);
            /* 						!NO RESTORES IN ENDGAME. */
            return ret_val;

        L78100:
            //rstrgm_();
            return ret_val;
        /* VAPPLI, PAGE 11 */

        /* V151--	HELLO */

        L80000:
            if (game.ParserVectors.prso != 0)
            {
                goto L80100;
            }

            /* 						!ANY OBJ? */
            i__1 = game.rnd_(4) + 346;
            MessageHandler.Speak(i__1, game);
            /* 						!NO, VANILLA HELLO. */
            return ret_val;

        L80100:
            if (game.ParserVectors.prso != (int)ObjectIndices.aviat)
            {
                goto L80200;
            }

            /* 						!HELLO AVIATOR? */
            MessageHandler.Speak(350, game);
            /* 						!NOTHING HAPPENS. */
            return ret_val;

        L80200:
            if (game.ParserVectors.prso != (int)ObjectIndices.sailo)
            {
                goto L80300;
            }

            /* 						!HELLO SAILOR? */
            ++game.State.hs;
            /* 						!COUNT. */
            i = 351;

            /* 						!GIVE NORMAL OR */
            if (game.State.hs % 10 == 0)
            {
                i = 352;
            }

            /* 						!RANDOM MESSAGE. */
            if (game.State.hs % 20 == 0)
            {
                i = 353;
            }

            MessageHandler.Speak(i, game);
            /* 						!SPEAK UP. */
            return ret_val;

        L80300:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }

            /* 						!OBJ HANDLE? */
            i = 354;
            /* 						!ASSUME VILLAIN. */
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & (int)ObjectFlags2.VILLBT + ObjectFlags2.ACTRBT) == 0)
            {
                i = 355;
            }

            MessageHandler.rspsub_(i, odo2, game);
            /* 						!HELLO THERE */
            /* 						! */
            return ret_val;

        /* V152--	LOOK INTO */

        L81000:
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }

            /* 						!OBJ HANDLE? */
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.DOORBT) == 0)
            {
                goto L81300;
            }

            if (!((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.OPENBT) != 0))
            {
                goto L81200;
            }

            /* 						!OPEN? */
            MessageHandler.rspsub_(628, odo2, game);
            /* 						!OPEN DOOR- UNINTERESTING. */
            return ret_val;

        L81200:
            MessageHandler.rspsub_(525, odo2, game);
            /* 						!CLOSED DOOR- CANT SEE. */
            return ret_val;

        L81300:
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.VEHBT) != 0)
            {
                goto L81400;
            }

            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.OPENBT) != 0
                || (game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.TRANBT) != 0)
            {
                goto L81400;
            }

            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.CONTBT) != 0)
            {
                goto L81200;
            }

            MessageHandler.rspsub_(630, odo2, game);
            /* 						!CANT LOOK INSIDE. */
            return ret_val;

        L81400:
            if (ObjectHandler.IsObjectEmpty(game.ParserVectors.prso, game))
            {
                goto L81500;
            }

            /* 						!VEH OR SEE IN.  EMPTY? */
            ObjectHandler.PrintDescription(game.ParserVectors.prso, 573, game);
            /* 						!NO, LIST CONTENTS. */
            return ret_val;

        L81500:
            MessageHandler.rspsub_(629, odo2, game);
            /* 						!EMPTY. */
            return ret_val;

        /* V153--	LOOK UNDER */

        L82000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.Speak(631, game);
            }

            /* 						!OBJECT HANDLE? */
            return ret_val;
        /* VAPPLI, PAGE 12 */

        /* V154--	PUMP */

        L83000:
            if (game.Objects.oroom[(int)ObjectIndices.pump - 1] == game.Player.Here || game.Objects.oadv[(int)ObjectIndices.pump - 1] == game.Player.Winner)
            {
                goto L83100;
            }

            MessageHandler.Speak(632, game);
            /* 						!NO. */
            return ret_val;

        L83100:
            game.ParserVectors.prsi = (int)ObjectIndices.pump;
            /* 						!BECOMES INFLATE */
            game.ParserVectors.prsa = (int)VIndices.inflaw;
            /* 						!X WITH PUMP. */
            goto L22000;
        /* 						!DONE. */

        /* V155--	WIND */

        L84000:
            if (!ObjectHandler.objact_(game))
            {
                MessageHandler.rspsub_(634, odo2, game);
            }
            /* 						!OBJ HANDLE? */
            return ret_val;

        /* V156--	CLIMB */
        /* V157--	CLIMB UP */
        /* V158--	CLIMB DOWN */

        L85000:
        L86000:
        L87000:
            i = (int)XSearch.xup;
            /* 						!ASSUME UP. */
            if (game.ParserVectors.prsa == (int)VIndices.clmbdw)
            {
                i = (int)XSearch.xdown;
            }
            /* 						!UNLESS CLIMB DN. */
            f = (game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.CLMBBT) != 0;
            if (f && dso3.findxt_(game, i, game.Player.Here))
            {
                goto L87500;
            }
            /* 						!ANYTHING TO CLIMB? */
            if (ObjectHandler.objact_(game))
            {
                return ret_val;
            }
            /* 						!OBJ HANDLE? */
            i = 657;
            if (f)
            {
                i = 524;
            }

            /* 						!VARIETY OF JOKES. */
            if (!f && (game.ParserVectors.prso == (int)ObjectIndices.wall
                || game.ParserVectors.prso >= (int)ObjectIndices.wnort
                && game.ParserVectors.prso <= (int)ObjectIndices.wnort + 3))
            {
                i = 656;
            }

            MessageHandler.Speak(i, game);
            /* 						!JOKE. */
            return ret_val;

        L87500:
            game.ParserVectors.prsa = (int)VIndices.walkw;
            /* 						!WALK */
            game.ParserVectors.prso = i;
            /* 						!IN SPECIFIED DIR. */
            ret_val = dverb2.walk_(game);
            return ret_val;
        }

        public static bool sverbs_(Game game, string input, int ri)
        {
            int mxnop = 39;
            int mxjoke = 64;
            int[] jokes = new int[] { 4, 5, 3, 304, 305, 306, 307, 308, 309, 310, 311, 312, 313, 5314, 5319, 324, 325, 883, 884, 120, 120, 0, 0, 0, 0 };
            int[] answer = new int[] { 0, 1, 2, 3, 4, 4, 4, 4, 5, 5, 5, 6, 7, 7 };
            string[] ansstr = new string[]
           { "TEMPLE", "FOREST", "30003", "FLASK", "RUB", "FONDLE",
      "CARRES", "TOUCH", "BONES", "BODY", "SKELE", "RUSTYKNIFE",
      "NONE", "NOWHER" };

            int i__1, i__2;
            bool ret_val;
            bool f;
            char z;
            char z2;
            int i, j;
            int k;
            int l;
            char[] ch = new char[1 * 6];
            int cp, wp;
            char[] pp1 = new char[1 * 6];
            char[] pp2 = new char[1 * 6];
            int odi2 = 0;
            int odo2 = 0;

            ret_val = true;
            /* 						!ASSUME WINS. */
            if (game.ParserVectors.prso != 0)
            {
                odo2 = game.Objects.odesc2[game.ParserVectors.prso - 1];
            }

            /* 						!SET UP DESCRIPTORS. */
            if (game.ParserVectors.prsi != 0)
            {
                odi2 = game.Objects.odesc2[game.ParserVectors.prsi - 1];
            }

            if (ri == 0)
            {
                throw new InvalidOperationException();
                // bug_(7, ri);
            }

            /* 						!ZERO IS VERBOTEN. */
            if (ri <= mxnop)
            {
                return ret_val;
            }

            /* 						!NOP? */
            if (ri <= mxjoke)
            {
                goto L100;
            }

            /* 						!JOKE? */
            switch (ri - mxjoke)
            {
                case 1: goto L65000;
                case 2: goto L66000;
                case 3: goto L67000;
                case 4: goto L68000;
                case 5: goto L69000;
                case 6: goto L1000;
                case 7: goto L2000;
                case 8: goto L3000;
                case 9: goto L4000;
                case 10: goto L5000;
                case 11: goto L6000;
                case 12: goto L7000;
                case 13: goto L8000;
                case 14: goto L9000;
                case 15: goto L10000;
                case 16: goto L11000;
                case 17: goto L12000;
                case 18: goto L13000;
                case 19: goto L14000;
                case 20: goto L15000;
                case 21: goto L16000;
                case 22: goto L17000;
                case 23: goto L18000;
                case 24: goto L19000;
                case 25: goto L20000;
                case 26: goto L21000;
                case 27: goto L22000;
                case 28: goto L23000;
                case 29: goto L24000;
                case 30: goto L25000;
                case 31: goto L26000;
                case 32: goto L27000;
            }

            throw new InvalidOperationException();
            //bug_(7, ri);

            /* ALL VERB PROCESSORS RETURN HERE TO DECLARE FAILURE. */

            /* L10: */
            ret_val = false;
            /* 						!LOSE. */
            return ret_val;

        /* JOKE PROCESSOR. */
        /* FIND PROPER ENTRY IN JOKES, USE IT TO SELECT STRING TO PRINT. */

        L100:
            i = jokes[ri - mxnop - 1];
            /* 						!GET TABLE ENTRY. */
            j = i / 1000;
            /* 						!ISOLATE # STRINGS. */
            if (j != 0)
            {
                i = i % 1000 + game.rnd_(j);
            }
            /* 						!IF RANDOM, CHOOSE. */
            MessageHandler.rspeak_(game, i);
            /* 						!PRINT JOKE. */
            return ret_val;
        /* SVERBS, PAGE 2A */

        /* V65--	ROOM */

        L65000:
            ret_val = RoomHandler.RoomDescription(game, 2);
            /* 						!DESCRIBE ROOM ONLY. */
            return ret_val;

        /* V66--	OBJECTS */

        L66000:
            ret_val = RoomHandler.RoomDescription(game, 1);
            /* 						!DESCRIBE OBJ ONLY. */
            if (!game.Player.TelFlag)
            {
                MessageHandler.rspeak_(game, 138);
            }
            /* 						!NO OBJECTS. */
            return ret_val;

        /* V67--	RNAME */

        L67000:
            i__1 = game.Rooms.RoomDescriptions2[game.Player.Here - 1];
            MessageHandler.rspeak_(game, i__1);
            /* 						!SHORT ROOM NAME. */
            return ret_val;

        /* V68--	RESERVED */

        L68000:
            return ret_val;

        /* V69--	RESERVED */

        L69000:
            return ret_val;
        /* SVERBS, PAGE 3 */

        /* V70--	BRIEF.  SET FLAG. */

        L1000:
            game.Flags.brieff = true;
            /* 						!BRIEF DESCRIPTIONS. */
            game.Flags.superf = false;
            MessageHandler.rspeak_(game, 326);
            return ret_val;

        /* V71--	VERBOSE.  CLEAR FLAGS. */

        L2000:
            game.Flags.brieff = false;
            /* 						!LONG DESCRIPTIONS. */
            game.Flags.superf = false;
            MessageHandler.rspeak_(game, 327);
            return ret_val;

        /* V72--	SUPERBRIEF.  SET FLAG. */

        L3000:
            game.Flags.superf = true;
            MessageHandler.rspeak_(game, 328);
            return ret_val;

        /* V73-- STAY (USED IN ENDGAME). */

        L4000:
            if (game.Player.Winner != (int)AIndices.amastr)
            {
                goto L4100;
            }
            /* 						!TELL MASTER, STAY. */
            MessageHandler.rspeak_(game, 781);
            /* 						!HE DOES. */
            game.Clock.Ticks[(int)ClockIndices.cevfol - 1] = 0;
            /* 						!NOT FOLLOWING. */
            return ret_val;

        L4100:
            if (game.Player.Winner == (int)AIndices.player)
            {
                MessageHandler.rspeak_(game, 664);
            }
            /* 						!JOKE. */
            return ret_val;

        /* V74--	VERSION.  PRINT INFO. */

        L5000:
            MessageHandler.more_output(string.Empty);
            //            Console.WriteLine("V%1d.%1d%c\n", vers_1.vmaj, vers_1.vmin, vers_1.vedit);
            game.Player.TelFlag = true;
            return ret_val;

        /* V75--	SWIM.  ALWAYS A JOKE. */

        L6000:
            i = 330;
            /* 						!ASSUME WATER. */
            if ((game.Rooms.RoomFlags[game.Player.Here - 1] & (int)RoomFlags.RWATER + RoomFlags.RFILL) == 0)
            {
                i = game.rnd_(3) + 331;
            }

            MessageHandler.rspeak_(game, i);
            return ret_val;

        /* V76--	GERONIMO.  IF IN BARREL, FATAL, ELSE JOKE. */

        L7000:
            if (game.Player.Here == (int)RoomIndices.mbarr)
            {
                goto L7100;
            }
            /* 						!IN BARREL? */
            MessageHandler.rspeak_(game, 334);
            /* 						!NO, JOKE. */
            return ret_val;

        L7100:
            AdventurerHandler.jigsup_(game, 335);
            /* 						!OVER FALLS. */
            return ret_val;

        /* V77--	SINBAD ET AL.  CHASE CYCLOPS, ELSE JOKE. */

        L8000:
            if (game.Player.Here == (int)RoomIndices.mcycl && ObjectHandler.qhere_(game, (int)ObjectIndices.cyclo, game.Player.Here))
            {
                goto L8100;
            }

            MessageHandler.rspeak_(game, 336);
            /* 						!NOT HERE, JOKE. */
            return ret_val;

        L8100:
            ObjectHandler.newsta_(game, (int)ObjectIndices.cyclo, 337, 0, 0, 0);
            /* 						!CYCLOPS FLEES. */
            game.Flags.cyclof = true;
            /* 						!SET ALL FLAGS. */
            game.Flags.magicf = true;
            game.Objects.oflag2[(int)ObjectIndices.cyclo - 1] &= ~ObjectFlags2.FITEBT;
            return ret_val;

        /* V78--	WELL.  OPEN DOOR, ELSE JOKE. */

        L9000:
            if (game.Flags.riddlf || game.Player.Here != (int)RoomIndices.riddl)
            {
                goto L9100;
            }
            /* 						!IN RIDDLE ROOM? */
            game.Flags.riddlf = true;
            /* 						!YES, SOLVED IT. */
            MessageHandler.rspeak_(game, 338);
            return ret_val;

        L9100:
            MessageHandler.rspeak_(game, 339);
            /* 						!WELL, WHAT? */
            return ret_val;

        /* V79--	PRAY.  IF IN TEMP2, POOF */
        /* 						! */

        L10000:
            if (game.Player.Here != (int)RoomIndices.temp2)
            {
                goto L10050;
            }
            /* 						!IN TEMPLE? */
            if (AdventurerHandler.moveto_(game, (int)RoomIndices.fore1, game.Player.Winner))
            {
                goto L10100;
            }
        /* 						!FORE1 STILL THERE? */
        L10050:
            MessageHandler.rspeak_(game, 340);
            /* 						!JOKE. */
            return ret_val;

        L10100:
            f = RoomHandler.RoomDescription(3, game);
            /* 						!MOVED, DESCRIBE. */
            return ret_val;

        /* V80--	TREASURE.  IF IN TEMP1, POOF */
        /* 						! */

        L11000:
            if (game.Player.Here != (int)RoomIndices.temp1)
            {
                goto L11050;
            }

            /* 						!IN TEMPLE? */
            if (AdventurerHandler.moveto_(game, (int)RoomIndices.treas, game.Player.Winner))
            {
                goto L10100;
            }
        /* 						!TREASURE ROOM THERE? */
        L11050:
            MessageHandler.rspeak_(game, 341);
            /* 						!NOTHING HAPPENS. */
            return ret_val;

        /* V81--	TEMPLE.  IF IN TREAS, POOF */
        /* 						! */

        L12000:
            if (game.Player.Here != (int)RoomIndices.treas)
            {
                goto L12050;
            }

            /* 						!IN TREASURE? */
            if (AdventurerHandler.moveto_(game, (int)RoomIndices.temp1, game.Player.Winner))
            {
                goto L10100;
            }
        /* 						!TEMP1 STILL THERE? */
        L12050:
            MessageHandler.rspeak_(game, 341);
            /* 						!NOTHING HAPPENS. */
            return ret_val;

        /* V82--	BLAST.  USUALLY A JOKE. */

        L13000:
            i = 342;
            /* 						!DONT UNDERSTAND. */
            if (game.ParserVectors.prso == (int)ObjectIndices.safe)
            {
                i = 252;
            }
            /* 						!JOKE FOR SAFE. */
            MessageHandler.rspeak_(game, i);
            return ret_val;

        /* V83--	SCORE.  PRINT SCORE. */

        L14000:
            AdventurerHandler.score_(game, false);
            return ret_val;

        /* V84--	QUIT.  FINISH OUT THE GAME. */

        L15000:
            AdventurerHandler.score_(game, true);
            /* 						!TELLL SCORE. */
            if (!dso3.yesno_(game, 343, 0, 0))
            {
                return ret_val;
            }
        /* 						!ASK FOR Y/N DECISION. */
        //exit_();
        /* 						!BYE. */
        /* SVERBS, PAGE 4 */

        /* V85--	FOLLOW (USED IN ENDGAME) */

        L16000:
            if (game.Player.Winner != (int)AIndices.amastr)
            {
                return ret_val;
            }

            /* 						!TELL MASTER, FOLLOW. */
            MessageHandler.rspeak_(game, 782);
            game.Clock.Ticks[(int)ClockIndices.cevfol - 1] = -1;
            /* 						!STARTS FOLLOWING. */
            return ret_val;

        /* V86--	WALK THROUGH */

        L17000:
            if (game.Screen.scolrm == 0
                || game.ParserVectors.prso != (int)ObjectIndices.scol
                && (game.ParserVectors.prso != (int)ObjectIndices.wnort
                || game.Player.Here != (int)RoomIndices.bkbox))
            {
                goto L17100;
            }

            game.Screen.scolac = game.Screen.scolrm;
            /* 						!WALKED THRU SCOL. */
            game.ParserVectors.prso = 0;
            /* 						!FAKE OUT FROMDR. */
            game.Clock.Ticks[(int)ClockIndices.cevscl - 1] = 6;
            /* 						!START ALARM. */
            MessageHandler.rspeak_(game, 668);
            /* 						!DISORIENT HIM. */
            f = AdventurerHandler.moveto_(game, game.Screen.scolrm, game.Player.Winner);
            /* 						!INTO ROOM. */
            f = RoomHandler.RoomDescription(game, 3);
            /* 						!DESCRIBE. */
            return ret_val;

        L17100:
            if (game.Player.Here != game.Screen.scolac)
            {
                goto L17300;
            }

            /* 						!ON OTHER SIDE OF SCOL? */
            for (i = 1; i <= 12; i += 3)
            {
                /* 						!WALK THRU PROPER WALL? */
                if (game.Screen.scolwl[i - 1] == game.Player.Here && game.Screen.scolwl[i] == game.ParserVectors.prso)
                {
                    goto L17500;
                }
                /* L17200: */
            }

        L17300:
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.TAKEBT) != 0)
            {
                goto L17400;
            }

            i = 669;
            /* 						!NO, JOKE. */
            if (game.ParserVectors.prso == (int)ObjectIndices.scol)
            {
                i = 670;
            }
            /* 						!SPECIAL JOKE FOR SCOL. */
            MessageHandler.rspsub_(game, i, odo2);
            return ret_val;

        L17400:
            i = 671;
            /* 						!JOKE. */
            if (game.Objects.oroom[game.ParserVectors.prso - 1] != 0)
            {
                i = game.rnd_(5) + 552;
            }
            /* 						!SPECIAL JOKES IF CARRY. */
            MessageHandler.rspeak_(game, i);
            return ret_val;

        L17500:
            game.ParserVectors.prso = game.Screen.scolwl[i + 1];
            /* 						!THRU SCOL WALL... */
            for (i = 1; i <= 8; i += 2)
            {
                /* 						!FIND MATCHING ROOM. */
                if (game.ParserVectors.prso == game.Screen.scoldr[i - 1])
                {
                    game.Screen.scolrm = game.Screen.scoldr[i];
                }
                /* L17600: */
            }
            /* 						!DECLARE NEW SCOLRM. */
            game.Clock.Ticks[(int)ClockIndices.cevscl - 1] = 0;
            /* 						!CANCEL ALARM. */
            MessageHandler.rspeak_(game, 668);
            /* 						!DISORIENT HIM. */
            f = AdventurerHandler.moveto_(game, (int)RoomIndices.bkbox, game.Player.Winner);
            /* 						!BACK IN BOX ROOM. */
            f = RoomHandler.RoomDescription(game, 3);
            return ret_val;

        /* V87--	RING.  A JOKE. */

        L18000:
            i = 359;
            /* 						!CANT RING. */
            if (game.ParserVectors.prso == (int)ObjectIndices.bell)
            {
                i = 360;
            }
            /* 						!DING, DONG. */
            MessageHandler.rspeak_(game, i);
            /* 						!JOKE. */
            return ret_val;

        /* V88--	BRUSH.  JOKE WITH OBSCURE TRAP. */

        L19000:
            if (game.ParserVectors.prso == (int)ObjectIndices.teeth)
            {
                goto L19100;
            }
            /* 						!BRUSH TEETH? */
            MessageHandler.rspeak_(game, 362);
            /* 						!NO, JOKE. */
            return ret_val;

        L19100:
            if (game.ParserVectors.prsi != 0)
            {
                goto L19200;
            }
            /* 						!WITH SOMETHING? */
            MessageHandler.rspeak_(game, 363);
            /* 						!NO, JOKE. */
            return ret_val;

        L19200:
            if (game.ParserVectors.prsi == (int)ObjectIndices.putty && game.Objects.oadv[(int)ObjectIndices.putty - 1] == game.Player.Winner)
            {
                goto L19300;
            }

            MessageHandler.rspsub_(game, 364, odi2);
            /* 						!NO, JOKE. */
            return ret_val;

        L19300:
            AdventurerHandler.jigsup_(game, 365);
            /* 						!YES, DEAD */
            /* 						! */
            /* 						! */
            /* 						! */
            /* 						! */
            /* 						! */
            return ret_val;
        /* SVERBS, PAGE 5 */

        /* V89--	DIG.  UNLESS SHOVEL, A JOKE. */

        L20000:
            if (game.ParserVectors.prso == (int)ObjectIndices.shove)
            {
                return ret_val;
            }

            /* 						!SHOVEL? */
            i = 392;
            /* 						!ASSUME TOOL. */
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.TOOLBT) == 0)
            {
                i = 393;
            }
            MessageHandler.rspsub_(game, i, odo2);
            return ret_val;

        /* V90--	TIME.  PRINT OUT DURATION OF GAME. */

        L21000:
            //gttime_(k);
            /* 						!GET PLAY TIME. */
            k = DateTime.Now.Second;
            i = k / 60;
            j = k % 60;

            MessageHandler.more_output(string.Empty);
            Console.Write("You have been playing Dungeon for ");
            if (i >= 1)
            {
                Console.Write("%d hour", i);
                if (i >= 2)
                    Console.Write("s");
                Console.Write(" and ");
            }
            Console.Write("%d minute", j);
            if (j != 1)
                Console.Write("s");
            Console.Write(".\n");
            game.Player.TelFlag = true;
            return ret_val;

        /* V91--	LEAP.  USUALLY A JOKE, WITH A CATCH. */

        L22000:
            if (game.ParserVectors.prso == 0)
            {
                goto L22200;
            }
            /* 						!OVER SOMETHING? */
            if (ObjectHandler.qhere_(game, game.ParserVectors.prso, game.Player.Here))
            {
                goto L22100;
            }
            /* 						!HERE? */
            MessageHandler.rspeak_(game, 447);
            /* 						!NO, JOKE. */
            return ret_val;

        L22100:
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.VILLBT) == 0)
            {
                goto L22300;
            }
            MessageHandler.rspsub_(game, 448, odo2);
            /* 						!CANT JUMP VILLAIN. */
            return ret_val;

        L22200:
            if (!dso3.findxt_(game, (int)XSearch.xdown, game.Player.Here))
            {
                goto L22300;
            }

            /* 						!DOWN EXIT? */
            if (game.curxt_.xtype == xpars_.xno || game.curxt_.xtype == xpars_.xcond)// && !game.Flags[xflag - 1])
            {
                goto L22400;
            }

        L22300:
            i__1 = game.rnd_(5) + 314;
            MessageHandler.rspeak_(game, i__1);
            /* 						!WHEEEE */
            /* 						! */
            return ret_val;

        L22400:
            i__1 = game.rnd_(4) + 449;
            AdventurerHandler.jigsup_(game, i__1);
            /* 						!FATAL LEAP. */
            return ret_val;
        /* SVERBS, PAGE 6 */

        /* V92--	LOCK. */

        L23000:
            if (game.ParserVectors.prso == (int)ObjectIndices.grate && game.Player.Here == (int)RoomIndices.mgrat)
            {
                goto L23200;
            }
        L23100:
            MessageHandler.rspeak_(game, 464);
            /* 						!NOT LOCK GRATE. */
            return ret_val;

        L23200:
            game.Flags.grunlf = false;
            /* 						!GRATE NOW LOCKED. */
            MessageHandler.rspeak_(game, 214);
            game.Exits.Travel[game.Rooms.RoomExits[game.Player.Here - 1]] = 214;
            /* 						!CHANGE EXIT STATUS. */
            return ret_val;

        /* V93--	UNLOCK */

        L24000:
            if (game.ParserVectors.prso != (int)ObjectIndices.grate || game.Player.Here != (int)RoomIndices.mgrat)
            {
                goto L23100;
            }
            if (game.ParserVectors.prsi == (int)ObjectIndices.keys)
            {
                goto L24200;
            }
            /* 						!GOT KEYS? */
            MessageHandler.rspsub_(game, 465, odi2);
            /* 						!NO, JOKE. */
            return ret_val;

        L24200:
            game.Flags.grunlf = true;
            /* 						!UNLOCK GRATE. */
            MessageHandler.rspeak_(game, 217);
            game.Exits.Travel[game.Rooms.RoomExits[game.Player.Here - 1]] = 217;
            /* 						!CHANGE EXIT STATUS. */
            return ret_val;

        /* V94--	DIAGNOSE. */

        L25000:
            i = dso4.fights_(game, game.Player.Winner, false);
            /* 						!GET FIGHTS STRENGTH. */
            j = game.Adventurers.astren[game.Player.Winner - 1];
            /* 						!GET HEALTH. */
            /* Computing MIN */
            i__1 = i + j;
            k = Math.Min(i__1, 4);
            /* 						!GET STATE. */
            if (!game.Clock.Flags[(int)ClockIndices.cevcur - 1])
            {
                j = 0;
            }
            /* 						!IF NO WOUNDS. */
            /* Computing MIN */
            i__1 = 4;
            i__2 = Math.Abs(j);
            l = Math.Min(i__1, i__2);
            /* 						!SCALE. */
            i__1 = l + 473;
            MessageHandler.rspeak_(game, i__1);
            /* 						!DESCRIBE HEALTH. */
            i = (-j - 1) * 30 + game.Clock.Ticks[(int)ClockIndices.cevcur - 1];
            /* 						!COMPUTE WAIT. */

            if (j != 0)
            {
                MessageHandler.more_output(string.Empty);
                Console.WriteLine("You will be cured after %d moves.\n", i);
            }

            i__1 = k + 478;
            MessageHandler.rspeak_(game, i__1);
            /* 						!HOW MUCH MORE? */
            if (game.State.Deaths != 0)
            {
                i__1 = game.State.Deaths + 482;
                MessageHandler.rspeak_(game, i__1);
            }
            /* 						!HOW MANY DEATHS? */
            return ret_val;
        /* SVERBS, PAGE 7 */

        /* V95--	INCANT */

        L26000:
            for (i = 1; i <= 6; ++i)
            {
                /* 						!SET UP PARSE. */
                pp1[i - 1] = ' ';
                pp2[i - 1] = ' ';
                /* L26100: */
            }
            wp = 1;
            /* 						!WORD POINTER. */
            cp = 1;
            /* 						!CHAR POINTER. */
            if (game.ParserVectors.prscon <= 1)
            {
                goto L26300;
            }

            for (z = (char)(input[0] + game.ParserVectors.prscon - 1); z != '\0'; ++z)
            {
                /* 						!PARSE INPUT */
                if (z == ',')
                    goto L26300;
                /* 						!END OF PHRASE? */
                if (z != ' ')
                    goto L26150;
                /* 						!SPACE? */
                if (cp != 1)
                {
                    ++wp;
                }
                cp = 1;
                goto L26200;
            L26150:
                if (wp == 1)
                {
                    pp1[cp - 1] = z;
                }
                /* 						!STUFF INTO HOLDER. */
                if (wp == 2)
                {
                    pp2[cp - 1] = z;
                }
                /* Computing MIN */
                i__2 = cp + 1;
                cp = Math.Min(i__2, 6);
            L26200:
                ;
            }

        L26300:
            game.ParserVectors.prscon = 1;
            /* 						!KILL REST OF LINE. */
            if (pp1[0] != ' ')
            {
                goto L26400;
            }
            /* 						!ANY INPUT? */
            MessageHandler.rspeak_(game, 856);
            /* 						!NO, HO HUM. */
            return ret_val;

        L26400:
            dso7.encryp_(game, pp1, ch);
            /* 						!COMPUTE RESPONSE. */
            if (pp2[0] != ' ')
            {
                goto L26600;
            }
            /* 						!TWO PHRASES? */

            if (game.Flags.spellf)
            {
                goto L26550;
            }

            /* 						!HE'S TRYING TO LEARN. */
            if ((game.Rooms.RoomFlags[(int)RoomIndices.tstrs - 1] & RoomFlags.RSEEN) == 0)
            {
                goto L26575;
            }

            game.Flags.spellf = true;
            /* 						!TELL HIM. */
            game.Player.TelFlag = true;
            MessageHandler.more_output(string.Empty);
            Console.WriteLine("A hollow voice replies:  \"%.6s %.6s\".\n", pp1, ch);

            return ret_val;

        L26550:
            MessageHandler.rspeak_(game, 857);
            /* 						!HE'S GOT ONE ALREADY. */
            return ret_val;

        L26575:
            MessageHandler.rspeak_(game, 858);
            /* 						!HE'S NOT IN ENDGAME. */
            return ret_val;

        L26600:
            if ((game.Rooms.RoomFlags[(int)RoomIndices.tstrs - 1] & RoomFlags.RSEEN) != 0)
            {
                goto L26800;
            }

            for (i = 1; i <= 6; ++i)
            {
                if (pp2[i - 1] != ch[i - 1])
                {
                    goto L26575;
                }
                /* 						!WRONG. */
                /* L26700: */
            }

            game.Flags.spellf = true;
            /* 						!IT WORKS. */
            MessageHandler.rspeak_(game, 859);
            game.Clock.Ticks[(int)ClockIndices.cevste - 1] = 1;
            /* 						!FORCE START. */
            return ret_val;

        L26800:
            MessageHandler.rspeak_(game, 855);
            /* 						!TOO LATE. */
            return ret_val;
        /* SVERBS, PAGE 8 */

        /* V96--	ANSWER */

        L27000:
            if (game.ParserVectors.prscon > 1
                && game.Player.Here == (int)RoomIndices.fdoor && game.Flags.inqstf)
            {
                goto L27100;
            }
            MessageHandler.rspeak_(game, 799);
            /* 						!NO ONE LISTENS. */
            game.ParserVectors.prscon = 1;
            return ret_val;

        L27100:
            for (j = 1; j <= 14; j++)
            {
                /* 						!CHECK ANSWERS. */
                if (game.Switches.quesno != answer[j - 1])
                {
                    goto L27300;
                }

                /* 						!ONLY CHECK PROPER ANS. */
                z = ansstr[j - 1][0];
                z2 = (char)(input[0] + game.ParserVectors.prscon - 1);

                while (z != '\0')
                {
                    while (z2 == ' ')
                        z2++;
                    /* 						!STRIP INPUT BLANKS. */
                    if (z++ != z2++)
                        goto L27300;
                }
                goto L27500;
            /* 						!RIGHT ANSWER. */
            L27300:
                ;
            }

            game.ParserVectors.prscon = 1;
            /* 						!KILL REST OF LINE. */
            ++game.Switches.nqatt;
            /* 						!WRONG, CRETIN. */
            if (game.Switches.nqatt >= 5)
            {
                goto L27400;
            }
            /* 						!TOO MANY WRONG? */
            i__1 = game.Switches.nqatt + 800;
            MessageHandler.rspeak_(game, i__1);
            /* 						!NO, TRY AGAIN. */
            return ret_val;

        L27400:
            MessageHandler.rspeak_(game, 826);
            /* 						!ALL OVER. */
            game.Clock.Flags[(int)ClockIndices.cevinq - 1] = false;
            /* 						!LOSE. */
            return ret_val;

        L27500:
            game.ParserVectors.prscon = 1;
            /* 						!KILL REST OF LINE. */
            ++game.Switches.corrct;
            /* 						!GOT IT RIGHT. */
            MessageHandler.rspeak_(game, 800);
            /* 						!HOORAY. */
            if (game.Switches.corrct >= 3)
            {
                goto L27600;
            }
            /* 						!WON TOTALLY? */
            game.Clock.Ticks[(int)ClockIndices.cevinq - 1] = 2;
            /* 						!NO, START AGAIN. */
            game.Switches.quesno = (game.Switches.quesno + 3) % 8;
            game.Switches.nqatt = 0;
            MessageHandler.rspeak_(game, 769);
            /* 						!ASK NEXT QUESTION. */
            i__1 = game.Switches.quesno + 770;
            MessageHandler.rspeak_(game, i__1);
            return ret_val;

        L27600:
            MessageHandler.rspeak_(game, 827);
            /* 						!QUIZ OVER, */
            game.Clock.Flags[(int)ClockIndices.cevinq - 1] = false;
            game.Objects.oflag2[(int)ObjectIndices.qdoor - 1] |= ObjectFlags2.OPENBT;
            return ret_val;
        }
    }

    public class pv
    {
        public int act { get; set; }//, o1, o2, p1, p2;
    }

    public class objvec
    {
        public int o1 { get; set; }
        public int o2 { get; set; }
    }

    public class prpvec
    {
        public int p1 { get; set; }
        public int p2 { get; set; }
    }
}