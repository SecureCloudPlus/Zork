﻿using System;
using Zork.Core.Clock;
using Zork.Core.Object;
using Zork.Core.Room;

namespace Zork.Core
{
    public static class ObjectHandler
    {
        public static bool IsInRoom(int roomId, int obj, Game game)
        {
            // System generated locals
            int i__1;
            bool ret_val;

            // Local variables
            int i;

            ret_val = true;
            if (game.Objects.oroom[obj - 1] == roomId)
            {
                return ret_val;
            }

            // !IN ROOM?
            i__1 = game.Rooms2.Count;
            for (i = 1; i <= i__1; ++i)
            {
                // !NO, SCH ROOM2.
                if (game.Rooms2.Rooms[i - 1] == obj && game.Rooms2.RRoom[i - 1] == roomId)
                {
                    return ret_val;
                }
                /* L100: */
            }
            ret_val = false;

            // !NOT PRESENT.
            return ret_val;
        }

        /// <summary>
        /// newsta_ - Set new status for object
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        public static void newsta_(int o, int r, int rm, int cn, int ad, Game game)
        {
            MessageHandler.Speak(r, game);
            game.Objects.oroom[o - 1] = rm;
            game.Objects.ocan[o - 1] = cn;
            game.Objects.oadv[o - 1] = ad;
        }

        public static void newsta_(Game game, int o, int r, int rm, int cn, int ad) => newsta_(o, r, rm, cn, ad, game);

        public static void newsta_(ObjectIndices objInd, int r, int rm, int cn, int ad, Game game) => newsta_((int)objInd, r, rm, cn, ad, game);

        public static void newsta_(ObjectIndices objInd, int r, int rm, ObjectIndices cn, int ad, Game game) => newsta_(objInd, r, (int)rm, cn, ad, game);

        public static void newsta_(ObjectIndices objInd, int r, RoomIndices rm, int cn, int ad, Game game) => newsta_(objInd, r, (int)rm, cn, ad, game);

        /// <summary>
        /// princo_ - PRINT CONTENTS OF OBJECT
        /// </summary>
        /// <param name="objectId"></param>
        /// <param name="descriptionId"></param>
        /// <param name="game"></param>
        public static void PrintDescription(int objectId, int descriptionId, Game game)
        {
            int i__1;

            // Local variables
            int i;

            MessageHandler.rspsub_(descriptionId, game.Objects.odesc2[objectId - 1], game);
            // !PRINT HEADER.
            i__1 = game.Objects.Count;
            for (i = 1; i <= i__1; ++i)
            {
                // !LOOP THRU.
                if (game.Objects.ocan[i - 1] == objectId)
                {
                    MessageHandler.rspsub_(502, game.Objects.odesc2[i - 1], game);
                }

                // L100:
            }
        }

        /// <summary>
        /// oactor_ - Get Actor associated with object.
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public static int GetActor(int objId, Game game)
        {
            int ret_val = 1;

            for (int i = 1; i <= game.Adventurers.Count; ++i)
            {
                // !LOOP THRU ACTORS.
                ret_val = i;
                // !ASSUME FOUND.
                if (game.Adventurers.Objects[i - 1] == objId)
                {
                    return ret_val;
                }

                // !FOUND IT?
                // L100:
            }

            throw new InvalidOperationException($"PROGRAM ERROR 40, PARAMETER={objId}");
            //bug_(40, obj);
            // !NO, DIE.
            return ret_val;
        }

        /// <summary>
        /// qempty_ Test for object empty
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public static bool IsObjectEmpty(int objId, Game game)
        {
            bool ret_val = false;

            // !ASSUME LOSE.
            for (int i = 1; i <= game.Objects.Count; ++i)
            {
                if (game.Objects.ocan[i - 1] == objId)
                {
                    return ret_val;
                }
            }

            ret_val = true;
            return ret_val;
        }

        public static bool IsObjectEmpty(ObjectIndices objId, Game game) => IsObjectEmpty((int)objId, game);

        /// <summary>
        /// qhere_ - Test for object in room
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="rm"></param>
        /// <param name="game"></param>
        /// <returns></returns>
        public static bool qhere_(int obj, int rm, Game game)
        {
            int i__1;
            bool ret_val;

            int i;

            ret_val = true;
            if (game.Objects.oroom[obj - 1] == rm)
            {
                return ret_val;
            }
            /* 						!IN ROOM? */
            i__1 = game.Rooms2.Count;
            for (i = 1; i <= i__1; ++i)
            {
                /* 						!NO, SCH ROOM2. */
                if (game.Rooms2.Rooms[i - 1] == obj && game.Rooms2.RRoom[i - 1] == rm)
                {
                    return ret_val;
                }
                /* L100: */
            }
            ret_val = false;
            /* 						!NOT PRESENT. */
            return ret_val;
        }

        /* WEIGHT- RETURNS SUM OF WEIGHT OF QUALIFYING OBJECTS */

        public static int weight_(int rm, int cn, int ad, Game game)
        {
            /* System generated locals */
            int ret_val, i__1;

            /* Local variables */
            int i, j;

            ret_val = 0;
            i__1 = game.Objects.Count;
            for (i = 1; i <= i__1; ++i)
            {
                /* 						!OMIT BIG FIXED ITEMS. */
                if (game.Objects.osize[i - 1] >= 10000)
                {
                    goto L100;
                }
                /* 						!IF FIXED, FORGET IT. */
                if (ObjectHandler.qhere_(i, rm, game) && rm != 0 || game.Objects.oadv[i - 1] == ad && ad != 0)
                {
                    goto L50;
                }

                j = i;
            /* 						!SEE IF CONTAINED. */
            L25:
                j = game.Objects.ocan[j - 1];
                /* 						!GET NEXT LEVEL UP. */
                if (j == 0)
                {
                    goto L100;
                }
                /* 						!END OF LIST? */
                if (j != cn)
                {
                    goto L25;
                }
            L50:
                ret_val += game.Objects.osize[i - 1];
            L100:
                ;
            }
            return ret_val;
        }

        /* QEMPTY-- TEST FOR OBJECT EMPTY */

        /* DECLARATIONS */

        public static bool qempty_(Game game, ObjectIndices obj)
        {
            /* System generated locals */
            int i__1;
            bool ret_val;

            /* Local variables */
            int i;

            ret_val = false;
            /* 						!ASSUME LOSE. */
            i__1 = game.Objects.Count;
            for (i = 1; i <= i__1; ++i)
            {
                if (game.Objects.ocan[i - 1] == (int)obj)
                {
                    return ret_val;
                }
                /* 						!INSIDE TARGET? */
                /* L100: */
            }
            ret_val = true;
            return ret_val;
        }

        /// <summary>
        /// qehre_ - Test for object in room.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="obj"></param>
        /// <param name="rm"></param>
        /// <returns></returns>
        public static bool qhere_(Game game, int obj, int rm)
        {
            /* System generated locals */
            int i__1;
            bool ret_val;

            /* Local variables */
            int i;

            ret_val = true;
            if (game.Objects.oroom[obj - 1] == rm)
            {
                return ret_val;
            }
            /* 						!IN ROOM? */
            i__1 = game.Rooms2.Count;
            for (i = 1; i <= i__1; ++i)
            {
                /* 						!NO, SCH ROOM2. */
                if (game.Rooms2.Rooms[i - 1] == obj && game.Rooms2.RRoom[i - 1] == rm)
                {
                    return ret_val;
                }
                /* L100: */
            }
            ret_val = false;
            /* 						!NOT PRESENT. */
            return ret_val;
        }

        public static bool nobjs_(Game game, int ri, int arg)
        {
            int i__1, i__2;
            bool ret_val;

            bool f;
            int target;
            int i;
            int j;
            int av, wl;
            int nxt, odi2 = 0, odo2 = 0;

            if (game.ParserVectors.prso != 0)
            {
                odo2 = game.Objects.odesc2[game.ParserVectors.prso - 1];
            }
            if (game.ParserVectors.prsi != 0)
            {
                odi2 = game.Objects.odesc2[game.ParserVectors.prsi - 1];
            }
            av = game.Adventurers.Vehicles[game.Player.Winner - 1];
            ret_val = true;

            switch (ri - 31)
            {
                case 1: goto L1000;
                case 2: goto L2000;
                case 3: goto L3000;
                case 4: goto L4000;
                case 5: goto L5000;
                case 6: goto L6000;
                case 7: goto L7000;
                case 8: goto L8000;
                case 9: goto L9000;
                case 10: goto L10000;
                case 11: goto L11000;
                case 12: goto L12000;
                case 13: goto L13000;
                case 14: goto L14000;
                case 15: goto L15000;
                case 16: goto L16000;
                case 17: goto L17000;
                case 18: goto L18000;
                case 19: goto L19000;
                case 20: goto L20000;
                case 21: goto L21000;
            }
            throw new InvalidOperationException();
        //bug_(6, ri);

        /* RETURN HERE TO DECLARE FALSE RESULT */

        L10:
            ret_val = false;
            return ret_val;

        /* O32--	BILLS */

        L1000:
            if (game.ParserVectors.prsa != (int)VIndices.eatw)
            {
                goto L1100;
            }
            /* 						!EAT? */
            MessageHandler.Speak(639, game);
            /* 						!JOKE. */
            return ret_val;

        L1100:
            if (game.ParserVectors.prsa == (int)VIndices.burnw)
            {
                MessageHandler.Speak(640, game);
            }
            /* 						!BURN?  JOKE. */
            goto L10;
        /* 						!LET IT BE HANDLED. */
        /* NOBJS, PAGE 3 */

        /* O33--	SCREEN OF LIGHT */

        L2000:
            target = (int)ObjectIndices.scol;
        /* 						!TARGET IS SCOL. */
        L2100:
            if (game.ParserVectors.prso != target)
            {
                goto L2400;
            }
            /* 						!PRSO EQ TARGET? */
            if (game.ParserVectors.prsa != (int)VIndices.pushw && game.ParserVectors.prsa != (int)VIndices.movew &&
                game.ParserVectors.prsa != (int)VIndices.takew && game.ParserVectors.prsa != (int)VIndices.rubw)
            {
                goto L2200;
            }
            MessageHandler.Speak(673, game);
            /* 						!HAND PASSES THRU. */
            return ret_val;

        L2200:
            if (game.ParserVectors.prsa != (int)VIndices.killw && game.ParserVectors.prsa != (int)VIndices.attacw &&
                 game.ParserVectors.prsa != (int)VIndices.mungw)
            {
                goto L2400;
            }
            MessageHandler.rspsub_(674, odi2, game);
            /* 						!PASSES THRU. */
            return ret_val;

        L2400:
            if (game.ParserVectors.prsa != (int)VIndices.throww || game.ParserVectors.prsi != target)
            {
                goto L10;
            }
            if (game.Player.Here == (int)RoomIndices.bkbox)
            {
                goto L2600;
            }
            /* 						!THRU SCOL? */
            newsta_(game.ParserVectors.prso, 0, (int)RoomIndices.bkbox, 0, 0, game);
            /* 						!NO, THRU WALL. */
            MessageHandler.rspsub_(675, odo2, game);
            /* 						!ENDS UP IN BOX ROOM. */
            game.Clock.Ticks[(int)ClockIndices.cevscl - 1] = 0;
            /* 						!CANCEL ALARM. */
            game.Screen.scolrm = 0;
            /* 						!RESET SCOL ROOM. */
            return ret_val;

        L2600:
            if (game.Screen.scolrm == 0)
            {
                goto L2900;
            }
            /* 						!TRIED TO GO THRU? */
            newsta_(game.ParserVectors.prso, 0, game.Screen.scolrm, 0, 0, game);
            /* 						!SUCCESS. */
            MessageHandler.rspsub_(676, odo2, game);
            /* 						!ENDS UP SOMEWHERE. */
            game.Clock.Ticks[(int)ClockIndices.cevscl - 1] = 0;
            /* 						!CANCEL ALARM. */
            game.Screen.scolrm = 0;
            /* 						!RESET SCOL ROOM. */
            return ret_val;

        L2900:
            MessageHandler.Speak(213, game);
            /* 						!CANT DO IT. */
            return ret_val;
        /* NOBJS, PAGE 4 */

        /* O34--	GNOME OF ZURICH */

        L3000:
            if (game.ParserVectors.prsa != (int)VIndices.givew && game.ParserVectors.prsa != (int)VIndices.throww)
            {
                goto L3200;
            }
            if (game.Objects.otval[game.ParserVectors.prso - 1] != 0)
            {
                goto L3100;
            }
            /* 						!THROW A TREASURE? */
            newsta_(game.ParserVectors.prso, 641, 0, 0, 0, game);
            /* 						!NO, GO POP. */
            return ret_val;

        L3100:
            newsta_(game.ParserVectors.prso, 0, 0, 0, 0, game);
            /* 						!YES, BYE BYE TREASURE. */
            MessageHandler.rspsub_(642, odo2, game);
            newsta_((int)ObjectIndices.zgnom, 0, 0, 0, 0, game);
            /* 						!BYE BYE GNOME. */
            game.Clock.Ticks[(int)ClockIndices.cevzgo - 1] = 0;
            /* 						!CANCEL EXIT. */
            f = AdventurerHandler.moveto_(game, RoomIndices.bkent, game.Player.Winner);
            /* 						!NOW IN BANK ENTRANCE. */
            return ret_val;

        L3200:
            if (game.ParserVectors.prsa != (int)VIndices.attacw && game.ParserVectors.prsa != (int)VIndices.killw &&
                 game.ParserVectors.prsa != (int)VIndices.mungw)
            {
                goto L3300;
            }
            newsta_((int)ObjectIndices.zgnom, 643, 0, 0, 0, game);
            /* 						!VANISH GNOME. */
            game.Clock.Ticks[(int)ClockIndices.cevzgo - 1] = 0;
            /* 						!CANCEL EXIT. */
            return ret_val;

        L3300:
            MessageHandler.Speak(644, game);
            /* 						!GNOME IS IMPATIENT. */
            return ret_val;

        /* O35--	EGG */

        L4000:
            if (game.ParserVectors.prsa != (int)VIndices.openw || game.ParserVectors.prso != (int)ObjectIndices.egg)
            {
                goto L4500;
            }

            if (!((game.Objects.oflag2[(int)ObjectIndices.egg - 1] & ObjectFlags2.OPENBT) != 0))
            {
                goto L4100;
            }

            /* 						!OPEN ALREADY? */
            MessageHandler.Speak(649, game);
            /* 						!YES. */
            return ret_val;

        L4100:
            if (game.ParserVectors.prsi != 0)
            {
                goto L4200;
            }
            /* 						!WITH SOMETHING? */
            MessageHandler.Speak(650, game);
            /* 						!NO, CANT. */
            return ret_val;

        L4200:
            if (game.ParserVectors.prsi != (int)ObjectIndices.hands)
            {
                goto L4300;
            }
            /* 						!WITH HANDS? */
            MessageHandler.Speak(651, game);
            /* 						!NOT RECOMMENDED. */
            return ret_val;

        L4300:
            i = 652;
            /* 						!MUNG MESSAGE. */
            if ((game.Objects.oflag1[game.ParserVectors.prsi - 1] & ObjectFlags.TOOLBT) != 0
                        || (game.Objects.oflag2[game.ParserVectors.prsi - 1] & ObjectFlags2.WEAPBT) != 0)
            {
                goto L4600;
            }

            i = 653;
            /* 						!NOVELTY 1. */
            if ((game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.FITEBT) != 0)
            {
                i = 654;
            }

            game.Objects.oflag2[game.ParserVectors.prso - 1] |= ObjectFlags2.FITEBT;
            MessageHandler.rspsub_(i, odi2, game);
            return ret_val;

        L4500:
            if (game.ParserVectors.prsa != (int)VIndices.openw && game.ParserVectors.prsa != (int)VIndices.mungw)
            {
                goto L4800;
            }
            i = 655;
        /* 						!YOU BLEW IT. */
        L4600:
            newsta_((int)ObjectIndices.begg, i, game.Objects.oroom[(int)ObjectIndices.egg - 1], game.Objects.ocan[(int)ObjectIndices.egg - 1], game.Objects.oadv[(int)ObjectIndices.egg - 1], game);
            newsta_((int)ObjectIndices.egg, 0, 0, 0, 0, game);
            /* 						!VANISH EGG. */
            game.Objects.otval[(int)ObjectIndices.begg - 1] = 2;
            /* 						!BAD EGG HAS VALUE. */
            if (game.Objects.ocan[(int)ObjectIndices.canar - 1] != (int)ObjectIndices.egg)
            {
                goto L4700;
            }
            /* 						!WAS CANARY INSIDE? */
            MessageHandler.Speak(game.Objects.odesco[(int)ObjectIndices.bcana - 1], game);
            /* 						!YES, DESCRIBE RESULT. */
            game.Objects.otval[(int)ObjectIndices.bcana - 1] = 1;
            return ret_val;

        L4700:
            newsta_((int)ObjectIndices.bcana, 0, 0, 0, 0, game);
            /* 						!NO, VANISH IT. */
            return ret_val;

        L4800:
            if (game.ParserVectors.prsa != (int)VIndices.dropw || game.Player.Here != (int)RoomIndices.mtree)
            {
                goto L10;
            }

            newsta_((int)ObjectIndices.begg, 658, (int)RoomIndices.fore3, 0, 0, game);
            /* 						!DROPPED EGG. */
            newsta_((int)ObjectIndices.egg, 0, 0, 0, 0, game);
            game.Objects.otval[(int)ObjectIndices.begg - 1] = 2;
            if (game.Objects.ocan[(int)ObjectIndices.canar - 1] != (int)ObjectIndices.egg)
            {
                goto L4700;
            }

            game.Objects.otval[(int)ObjectIndices.bcana - 1] = 1;
            /* 						!BAD CANARY. */
            return ret_val;
        /* NOBJS, PAGE 5 */

        /* O36--	CANARIES, GOOD AND BAD */

        L5000:
            if (game.ParserVectors.prsa != (int)VIndices.windw)
            {
                goto L10;
            }
            /* 						!WIND EM UP? */
            if (game.ParserVectors.prso == (int)ObjectIndices.canar)
            {
                goto L5100;
            }
            /* 						!RIGHT ONE? */
            MessageHandler.Speak(645, game);
            /* 						!NO, BAD NEWS. */
            return ret_val;

        L5100:
            if (!game.Flags.singsf && (game.Player.Here == (int)RoomIndices.mtree || game.Player.Here >= (int)RoomIndices.fore1 && game.Player.Here < (int)RoomIndices.clear))
            {
                goto L5200;
            }

            MessageHandler.Speak(646, game);
            /* 						!NO, MEDIOCRE NEWS. */
            return ret_val;

        L5200:
            game.Flags.singsf = true;
            /* 						!SANG SONG. */
            i = game.Player.Here;
            if (i == (int)RoomIndices.mtree)
            {
                i = (int)RoomIndices.fore3;
            }
            /* 						!PLACE BAUBLE. */
            newsta_((int)ObjectIndices.baubl, 647, i, 0, 0, game);
            return ret_val;

        /* O37--	WHITE CLIFFS */

        L6000:
            if (game.ParserVectors.prsa != (int)VIndices.clmbw && game.ParserVectors.prsa != (int)VIndices.clmbuw &&
                 game.ParserVectors.prsa != (int)VIndices.clmbdw)
            {
                goto L10;
            }
            MessageHandler.Speak(648, game);
            /* 						!OH YEAH? */
            return ret_val;

        /* O38--	WALL */
        L7000:
            i__1 = game.Player.Here - game.Switches.mloc;
            if (Math.Abs(i__1) != 1
                || RoomHandler.mrhere_(game, game.Player.Here) != 0
                || game.ParserVectors.prsa != (int)VIndices.pushw)
            {
                goto L7100;
            }

            MessageHandler.Speak(860, game);
            /* 						!PUSHED MIRROR WALL. */
            return ret_val;

        L7100:
            if ((game.Rooms.RoomFlags[game.Player.Here - 1] & RoomFlags.RNWALL) == 0)
            {
                goto L10;
            }
            MessageHandler.Speak(662, game);
            /* 						!NO WALL. */
            return ret_val;
        /* NOBJS, PAGE 6 */

        /* O39--	SONG BIRD GLOBAL */

        L8000:
            if (game.ParserVectors.prsa != (int)VIndices.findw)
            {
                goto L8100;
            }
            /* 						!FIND? */
            MessageHandler.Speak(666, game);
            return ret_val;

        L8100:
            if (game.ParserVectors.prsa != (int)VIndices.examiw)
            {
                goto L10;
            }
            /* 						!EXAMINE? */
            MessageHandler.Speak(667, game);
            return ret_val;

        /* O40--	PUZZLE/SCOL WALLS */

        L9000:
            if (game.Player.Here != (int)RoomIndices.cpuzz)
            {
                goto L9500;
            }
            /* 						!PUZZLE WALLS? */
            if (game.ParserVectors.prsa != (int)VIndices.pushw)
            {
                goto L10;
            }
            /* 						!PUSH? */
            for (i = 1; i <= 8; i += 2)
            {
                /* 						!LOCATE WALL. */
                if (game.ParserVectors.prso == PuzzleHandler.cpwl[i - 1])
                {
                    goto L9200;
                }
                /* L9100: */
            }
            throw new InvalidOperationException();
        //bug_(80, game.ParserVectors.prso);
        /* 						!WHAT? */

        L9200:
            j = PuzzleHandler.cpwl[i];
            /* 						!GET DIRECTIONAL OFFSET. */
            nxt = game.Switches.cphere + j;
            /* 						!GET NEXT STATE. */
            wl = PuzzleHandler.cpvec[nxt - 1];
            /* 						!GET C(NEXT STATE). */
            switch (wl + 4)
            {
                case 1: goto L9300;
                case 2: goto L9300;
                case 3: goto L9300;
                case 4: goto L9250;
                case 5: goto L9350;
            }
        /* 						!PROCESS. */

        L9250:
            MessageHandler.Speak(876, game);
            /* 						!CLEAR CORRIDOR. */
            return ret_val;

        L9300:
            if (PuzzleHandler.cpvec[nxt + j - 1] == 0)
            {
                goto L9400;
            }
        /* 						!MOVABLE, ROOM TO MOVE? */
        L9350:
            MessageHandler.Speak(877, game);
            /* 						!IMMOVABLE, NO ROOM. */
            return ret_val;

        L9400:
            i = 878;
            /* 						!ASSUME FIRST PUSH. */
            if (game.Flags.cpushf)
            {
                i = 879;
            }
            /* 						!NOT? */
            game.Flags.cpushf = true;
            PuzzleHandler.cpvec[nxt + j - 1] = wl;
            /* 						!MOVE WALL. */
            PuzzleHandler.cpvec[nxt - 1] = 0;
            /* 						!VACATE NEXT STATE. */
            dso7.cpgoto_(game, nxt);
            /* 						!ONWARD. */
            dso7.cpinfo_(game, i, nxt);
            /* 						!DESCRIBE. */
            RoomHandler.PrintRoomContents(true, game.Player.Here, game);
            /* 						!PRINT ROOMS CONTENTS. */
            game.Rooms.RoomFlags[game.Player.Here - 1] |= RoomFlags.RSEEN;
            return ret_val;

        L9500:
            if (game.Player.Here != game.Screen.scolac)
            {
                goto L9700;
            }
            /* 						!IN SCOL ACTIVE ROOM? */
            for (i = 1; i <= 12; i += 3)
            {
                target = game.Screen.scolwl[i];
                /* 						!ASSUME TARGET. */
                if (game.Screen.scolwl[i - 1] == game.Player.Here)
                {
                    goto L2100;
                }
                /* 						!TREAT IF FOUND. */
                /* L9600: */
            }

        L9700:
            if (game.Player.Here != (int)RoomIndices.bkbox)
            {
                goto L10;
            }
            /* 						!IN BOX ROOM? */
            target = (int)ObjectIndices.wnort;
            goto L2100;
        /* NOBJS, PAGE 7 */

        /* O41--	SHORT POLE */

        L10000:
            if (game.ParserVectors.prsa != (int)VIndices.raisew)
            {
                goto L10100;
            }
            /* 						!LIFT? */
            i = 749;
            /* 						!ASSUME UP. */
            if (game.Switches.poleuf == 2)
            {
                i = 750;
            }
            /* 						!ALREADY UP? */
            MessageHandler.Speak(i, game);
            game.Switches.poleuf = 2;
            /* 						!POLE IS RAISED. */
            return ret_val;

        L10100:
            if (game.ParserVectors.prsa != (int)VIndices.lowerw && game.ParserVectors.prsa != (int)VIndices.pushw)
            {
                goto L10;
            }
            if (game.Switches.poleuf != 0)
            {
                goto L10200;
            }
            /* 						!ALREADY LOWERED? */
            MessageHandler.Speak(751, game);
            /* 						!CANT DO IT. */
            return ret_val;

        L10200:
            if (game.Switches.mdir % 180 != 0)
            {
                goto L10300;
            }
            /* 						!MIRROR N-S? */
            game.Switches.poleuf = 0;
            /* 						!YES, LOWER INTO */
            MessageHandler.Speak(752, game);
            /* 						!CHANNEL. */
            return ret_val;

        L10300:
            if (game.Switches.mdir != 270 || game.Switches.mloc != (int)RoomIndices.mrb)
            {
                goto L10400;
            }
            game.Switches.poleuf = 0;
            /* 						!LOWER INTO HOLE. */
            MessageHandler.Speak(753, game);
            return ret_val;

        L10400:
            i__1 = game.Switches.poleuf + 753;
            MessageHandler.Speak(i__1, game);
            /* 						!POLEUF = 1 OR 2. */
            game.Switches.poleuf = 1;
            /* 						!NOW ON FLOOR. */
            return ret_val;

        /* O42--	MIRROR SWITCH */

        L11000:
            if (game.ParserVectors.prsa != (int)VIndices.pushw)
            {
                goto L10;
            }
            /* 						!PUSH? */
            if (game.Flags.mrpshf)
            {
                goto L11300;
            }
            /* 						!ALREADY PUSHED? */
            MessageHandler.Speak(756, game);
            /* 						!BUTTON GOES IN. */
            i__1 = game.Objects.Count;
            for (i = 1; i <= i__1; ++i)
            {
                /* 						!BLOCKED? */
                if (qhere_(i, (int)RoomIndices.mreye, game) && i != (int)ObjectIndices.rbeam)
                {
                    goto L11200;
                }
                /* L11100: */
            }
            MessageHandler.Speak(757, game);
            /* 						!NOTHING IN BEAM. */
            return ret_val;

        L11200:
            game.Clock.Flags[(int)ClockIndices.cevmrs - 1] = true;
            /* 						!MIRROR OPENS. */
            game.Clock.Ticks[(int)ClockIndices.cevmrs - 1] = 7;
            game.Flags.mrpshf = true;
            game.Flags.mropnf = true;
            return ret_val;

        L11300:
            MessageHandler.Speak(758, game);
            /* 						!MIRROR ALREADYOPEN. */
            return ret_val;
        /* NOBJS, PAGE 8 */

        /* O43--	BEAM FUNCTION */

        L12000:
            if (game.ParserVectors.prsa != (int)VIndices.takew || game.ParserVectors.prso != (int)ObjectIndices.rbeam)
            {
                goto L12100;
            }
            MessageHandler.Speak(759, game);
            /* 						!TAKE BEAM, JOKE. */
            return ret_val;

        L12100:
            i = game.ParserVectors.prso;
            /* 						!ASSUME BLK WITH DIROBJ. */
            if (game.ParserVectors.prsa == (int)VIndices.putw && game.ParserVectors.prsi == (int)ObjectIndices.rbeam)
            {
                goto L12200;
            }
            if (game.ParserVectors.prsa != (int)VIndices.mungw || game.ParserVectors.prso != (int)ObjectIndices.rbeam ||
                game.ParserVectors.prsi == 0)
            {
                goto L10;
            }
            i = game.ParserVectors.prsi;
        L12200:
            if (game.Objects.oadv[i - 1] != game.Player.Winner)
            {
                goto L12300;
            }
            /* 						!CARRYING? */
            newsta_(i, 0, game.Player.Here, 0, 0, game);
            /* 						!DROP OBJ. */
            MessageHandler.rspsub_(760, game.Objects.odesc2[i - 1], game);
            return ret_val;

        L12300:
            j = 761;
            /* 						!ASSUME NOT IN ROOM. */
            if (qhere_(j, game.Player.Here, game))
            {
                i = 762;
            }
            /* 						!IN ROOM? */
            MessageHandler.rspsub_(j, game.Objects.odesc2[i - 1], game);
            /* 						!DESCRIBE. */
            return ret_val;

        /* O44--	BRONZE DOOR */

        L13000:
            if (game.Player.Here == (int)RoomIndices.ncell || game.Switches.lcell == 4 && (game.Player.Here == (int)RoomIndices.cell || game.Player.Here == (int)RoomIndices.scorr))
            {
                goto L13100;
            }

            MessageHandler.Speak(763, game);
            /* 						!DOOR NOT THERE. */
            return ret_val;

        L13100:
            if (!RoomHandler.opncls_((int)ObjectIndices.odoor, 764, 765, game))
            {
                goto L10;
            }
            /* 						!OPEN/CLOSE? */
            if (game.Player.Here == (int)RoomIndices.ncell && (game.Objects.oflag2[(int)ObjectIndices.odoor - 1] & ObjectFlags2.OPENBT) != 0)
            {
                MessageHandler.Speak(766, game);
            }
            return ret_val;

        /* O45--	QUIZ DOOR */

        L14000:
            if (game.ParserVectors.prsa != (int)VIndices.openw && game.ParserVectors.prsa != (int)VIndices.closew)
            {
                goto L14100;
            }
            MessageHandler.Speak(767, game);
            /* 						!DOOR WONT MOVE. */
            return ret_val;

        L14100:
            if (game.ParserVectors.prsa != (int)VIndices.knockw)
            {
                goto L10;
            }
            /* 						!KNOCK? */
            if (game.Flags.inqstf)
            {
                goto L14200;
            }
            /* 						!TRIED IT ALREADY? */
            game.Flags.inqstf = true;
            /* 						!START INQUISITION. */
            game.Clock.Flags[(int)ClockIndices.cevinq - 1] = true;
            game.Clock.Ticks[(int)ClockIndices.cevinq - 1] = 2;
            game.Switches.quesno = game.rnd_(8);
            /* 						!SELECT QUESTION. */
            game.Switches.nqatt = 0;
            game.Switches.corrct = 0;
            MessageHandler.Speak(768, game);
            /* 						!ANNOUNCE RULES. */
            MessageHandler.Speak(769, game);
            i__1 = game.Switches.quesno + 770;
            MessageHandler.Speak(i__1, game);
            /* 						!ASK QUESTION. */
            return ret_val;

        L14200:
            MessageHandler.Speak(798, game);
            /* 						!NO REPLY. */
            return ret_val;

        /* O46--	LOCKED DOOR */

        L15000:
            if (game.ParserVectors.prsa != (int)VIndices.openw)
            {
                goto L10;
            }
            /* 						!OPEN? */
            MessageHandler.Speak(778, game);
            /* 						!CANT. */
            return ret_val;

        /* O47--	CELL DOOR */

        L16000:
            ret_val = RoomHandler.opncls_((int)ObjectIndices.cdoor, 779, 780, game);
            /* 						!OPEN/CLOSE? */
            return ret_val;
        /* NOBJS, PAGE 9 */

        /* O48--	DIALBUTTON */

        L17000:
            if (game.ParserVectors.prsa != (int)VIndices.pushw)
            {
                goto L10;
            }
            /* 						!PUSH? */
            MessageHandler.Speak(809, game);
            /* 						!CLICK. */
            if ((game.Objects.oflag2[(int)ObjectIndices.cdoor - 1] & ObjectFlags2.OPENBT) != 0)
            {
                MessageHandler.Speak(810, game);
            }
            /* 						!CLOSE CELL DOOR. */

            i__1 = game.Objects.Count;
            for (i = 1; i <= i__1; ++i)
            {
                /* 						!RELOCATE OLD TO HYPER. */
                if (game.Objects.oroom[i - 1] == (int)RoomIndices.cell && (game.Objects.oflag1[i - 1] & ObjectFlags.DOORBT) == 0)
                {
                    i__2 = game.Switches.lcell * game.hyper_.hfactr;
                    newsta_(i, 0, i__2, 0, 0, game);
                }

                if (game.Objects.oroom[i - 1] == game.Switches.pnumb * game.hyper_.hfactr)
                {
                    newsta_(i, 0, (int)RoomIndices.cell, 0, 0, game);
                }
                /* L17100: */
            }

            game.Objects.oflag2[(int)ObjectIndices.odoor - 1] &= ~ObjectFlags2.OPENBT;
            game.Objects.oflag2[(int)ObjectIndices.cdoor - 1] &= ~ObjectFlags2.OPENBT;
            game.Objects.oflag1[(int)ObjectIndices.odoor - 1] &= ~ObjectFlags.VISIBT;
            if (game.Switches.pnumb == 4)
            {
                game.Objects.oflag1[(int)ObjectIndices.odoor - 1] |= ObjectFlags.VISIBT;
            }

            if (game.Adventurers.Rooms[(int)AIndices.player - 1] != (int)RoomIndices.cell)
            {
                goto L17400;
            }
            /* 						!PLAYER IN CELL? */
            if (game.Switches.lcell != 4)
            {
                goto L17200;
            }
            /* 						!IN RIGHT CELL? */
            game.Objects.oflag1[(int)ObjectIndices.odoor - 1] |= ObjectFlags.VISIBT;
            f = AdventurerHandler.moveto_(game, RoomIndices.ncell, (int)AIndices.player);
            /* 						!YES, MOVETO NCELL. */
            goto L17400;
        L17200:
            f = AdventurerHandler.moveto_(game, RoomIndices.pcell, (int)AIndices.player);
        /* 						!NO, MOVETO PCELL. */

        L17400:
            game.Switches.lcell = game.Switches.pnumb;
            return ret_val;
        /* NOBJS, PAGE 10 */

        /* O49--	DIAL INDICATOR */

        L18000:
            if (game.ParserVectors.prsa != (int)VIndices.spinw)
            {
                goto L18100;
            }
            /* 						!SPIN? */
            game.Switches.pnumb = game.rnd_(8) + 1;
            /* 						!WHEE */
            /* 						! */
            i__1 = game.Switches.pnumb + 712;
            MessageHandler.rspsub_(797, i__1, game);
            return ret_val;

        L18100:
            if (game.ParserVectors.prsa != (int)VIndices.movew && game.ParserVectors.prsa != (int)VIndices.putw &&
                game.ParserVectors.prsa != (int)VIndices.trntow)
            {
                goto L10;
            }
            if (game.ParserVectors.prsi != 0)
            {
                goto L18200;
            }
            /* 						!TURN DIAL TO X? */
            MessageHandler.Speak(806, game);
            /* 						!MUST SPECIFY. */
            return ret_val;

        L18200:
            if (game.ParserVectors.prsi >= (int)ObjectIndices.num1 && game.ParserVectors.prsi <= (int)ObjectIndices.num8)
            {
                goto L18300;
            }
            MessageHandler.Speak(807, game);
            /* 						!MUST BE DIGIT. */
            return ret_val;

        L18300:
            game.Switches.pnumb = game.ParserVectors.prsi - (int)ObjectIndices.num1 + 1;
            /* 						!SET UP NEW. */
            i__1 = game.Switches.pnumb + 712;
            MessageHandler.rspsub_(808, i__1, game);
            return ret_val;

        /* O50--	GLOBAL MIRROR */

        L19000:
            ret_val = mirpan_(game, 832, false);
            return ret_val;

        /* O51--	GLOBAL PANEL */

        L20000:
            if (game.Player.Here != (int)RoomIndices.fdoor)
            {
                goto L20100;
            }
            /* 						!AT FRONT DOOR? */
            if (game.ParserVectors.prsa != (int)VIndices.openw && game.ParserVectors.prsa != (int)VIndices.closew)
            {
                goto L10;
            }
            MessageHandler.Speak(843, game);
            /* 						!PANEL IN DOOR, NOGO. */
            return ret_val;

        L20100:
            ret_val = mirpan_(game, 838, true);
            return ret_val;

        /* O52--	PUZZLE ROOM SLIT */

        L21000:
            if (game.ParserVectors.prsa != (int)VIndices.putw || game.ParserVectors.prsi != (int)ObjectIndices.cslit)
            {
                goto L10;
            }
            if (game.ParserVectors.prso != (int)ObjectIndices.gcard)
            {
                goto L21100;
            }
            /* 						!PUT CARD IN SLIT? */
            newsta_(game.ParserVectors.prso, 863, 0, 0, 0, game);
            /* 						!KILL CARD. */
            game.Flags.cpoutf = true;
            /* 						!OPEN DOOR. */
            game.Objects.oflag1[(int)ObjectIndices.stldr - 1] &= ~ObjectFlags.VISIBT;
            return ret_val;

        L21100:
            if ((game.Objects.oflag1[game.ParserVectors.prso - 1] & ObjectFlags.VICTBT) == 0
                && (game.Objects.oflag2[game.ParserVectors.prso - 1] & ObjectFlags2.VILLBT) == 0)
            {
                goto L21200;
            }

            i__1 = game.rnd_(5) + 552;
            MessageHandler.Speak(i__1, game);
            /* 						!JOKE FOR VILL, VICT. */
            return ret_val;

        L21200:
            newsta_(game.ParserVectors.prso, 0, 0, 0, 0, game);
            /* 						!KILL OBJECT. */
            MessageHandler.rspsub_(864, odo2, game);
            /* 						!DESCRIBE. */
            return ret_val;
        }

        /// <summary>
        /// objact_ - Apply objects from parse vector
        /// </summary>
        /// <returns></returns>
        public static bool objact_(Game game)
        {
            bool ret_val;

            ret_val = true;
            /* 						!ASSUME WINS. */
            if (game.ParserVectors.prsi == 0)
            {
                goto L100;
            }
            /* 						!IND OBJECT? */
            if (oappli_(game.Objects.oactio[game.ParserVectors.prsi - 1], 0, game))
            {
                return ret_val;
            }
        /* 						!YES, LET IT HANDLE. */

        L100:
            if (game.ParserVectors.prso == 0)
            {
                goto L200;
            }

            /* 						!DIR OBJECT? */
            if (oappli_(game.Objects.oactio[game.ParserVectors.prso - 1], 0, game))
            {
                return ret_val;
            }
        /* 						!YES, LET IT HANDLE. */

        L200:
            ret_val = false;

            /* 						!LOSES. */
            return ret_val;
        }

        /// <summary>
        /// oappli_ - Object special action routines
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static bool oappli_(int ri, int arg, Game game)
        {
            const int mxsmp = 99;

            int i__1;
            bool ret_val;

            bool f;
            int flobts, i;
            int j, av, io, ir, iz;
            int odi2 = 0, odo2 = 0;
            int nloc;

            if (ri == 0)
            {
                goto L10;
            }
            /* 						!ZERO IS FALSE APP. */
            if (ri <= mxsmp)
            {
                goto L100;
            }
            /* 						!SIMPLE OBJECT? */
            if (game.ParserVectors.prso > 220)
            {
                goto L5;
            }
            if (game.ParserVectors.prso != 0)
            {
                odo2 = game.Objects.odesc2[game.ParserVectors.prso - 1];
            }
        L5:
            if (game.ParserVectors.prsi != 0)
            {
                odi2 = game.Objects.odesc2[game.ParserVectors.prsi - 1];
            }
            av = game.Adventurers.Vehicles[game.Player.Winner - 1];
            flobts = (int)(ObjectFlags.FLAMBT + (int)ObjectFlags.LITEBT + (int)ObjectFlags.ONBT);
            ret_val = true;

            switch (ri - mxsmp)
            {
                case 1: goto L2000;
                case 2: goto L5000;
                case 3: goto L10000;
                case 4: goto L11000;
                case 5: goto L12000;
                case 6: goto L15000;
                case 7: goto L18000;
                case 8: goto L19000;
                case 9: goto L20000;
                case 10: goto L22000;
                case 11: goto L25000;
                case 12: goto L26000;
                case 13: goto L32000;
                case 14: goto L35000;
                case 15: goto L39000;
                case 16: goto L40000;
                case 17: goto L45000;
                case 18: goto L47000;
                case 19: goto L48000;
                case 20: goto L49000;
                case 21: goto L50000;
                case 22: goto L51000;
                case 23: goto L52000;
                case 24: goto L54000;
                case 25: goto L55000;
                case 26: goto L56000;
                case 27: goto L57000;
                case 28: goto L58000;
                case 29: goto L59000;
                case 30: goto L60000;
                case 31: goto L61000;
                case 32: goto L62000;
            }

            throw new InvalidOperationException("6");
        //            bug_(6, ri);

        /* RETURN HERE TO DECLARE FALSE RESULT */

        L10:
            ret_val = false;
            return ret_val;

        /* SIMPLE OBJECTS, PROCESSED EXTERNALLY. */

        L100:
            if (ri < 32)
            {
                ret_val = sobjs_(game, ri, arg);
            }
            else
            {
                ret_val = ObjectHandler.nobjs_(game, ri, arg);
            }
            return ret_val;
        /* OAPPLI, PAGE 3 */

        /* O100--	MACHINE FUNCTION */

        L2000:
            if (game.Player.Here != (int)RoomIndices.mmach)
            {
                goto L10;
            }
            /* 						!NOT HERE? F */
            ret_val = RoomHandler.opncls_((int)ObjectIndices.machi, 123, 124, game);
            /* 						!HANDLE OPN/CLS. */
            return ret_val;

        /* O101--	WATER FUNCTION */

        L5000:
            if (game.ParserVectors.prsa != (int)VIndices.fillw)
            {
                goto L5050;
            }
            /* 						!FILL X WITH Y IS */
            game.ParserVectors.prsa = (int)VIndices.putw;
            /* 						!MADE INTO */
            i = game.ParserVectors.prsi;
            game.ParserVectors.prsi = game.ParserVectors.prso;
            game.ParserVectors.prso = i;
            /* 						!PUT Y IN X. */
            i = odi2;
            odi2 = odo2;
            odo2 = i;
        L5050:
            if (game.ParserVectors.prso == (int)ObjectIndices.water || game.ParserVectors.prso == (int)ObjectIndices.gwate)
            {
                goto L5100;
            }
            MessageHandler.Speak(561, game);
            /* 						!WATER IS IND OBJ, */
            return ret_val;
        /* 						!PUNT. */

        L5100:
            if (game.ParserVectors.prsa != (int)VIndices.takew)
            {
                goto L5400;
            }
            /* 						!TAKE WATER? */
            if (game.Objects.oadv[(int)ObjectIndices.bottl - 1] == game.Player.Winner && game.Objects.ocan[game.ParserVectors.prso - 1] != (int)ObjectIndices.bottl)
            {
                goto L5500;
            }
            if (game.Objects.ocan[game.ParserVectors.prso - 1] == 0)
            {
                goto L5200;
            }
            /* 						!INSIDE ANYTHING? */
            if ((game.Objects.oflag2[game.Objects.ocan[game.ParserVectors.prso - 1] - 1] & ObjectFlags2.OPENBT) != 0)
            {
                goto L5200;
            }
            /* 						!YES, OPEN? */
            MessageHandler.rspsub_(525, game.Objects.odesc2[game.Objects.ocan[game.ParserVectors.prso - 1] - 1], game);
            /* 						!INSIDE, CLOSED, PUNT. */
            return ret_val;

        L5200:
            MessageHandler.Speak(615, game);
            /* 						!NOT INSIDE OR OPEN, */
            return ret_val;
        /* 						!SLIPS THRU FINGERS. */

        L5400:
            if (game.ParserVectors.prsa != (int)VIndices.putw)
            {
                goto L5700;
            }
            /* 						!PUT WATER IN X? */
            if (av != 0 && game.ParserVectors.prsi == av)
            {
                goto L5800;
            }
            /* 						!IN VEH? */
            if (game.ParserVectors.prsi == (int)ObjectIndices.bottl)
            {
                goto L5500;
            }
            /* 						!IN BOTTLE? */
            MessageHandler.rspsub_(297, odi2, game);
            /* 						!WONT GO ELSEWHERE. */
            newsta_(game.ParserVectors.prso, 0, 0, 0, 0, game);
            /* 						!VANISH WATER. */
            return ret_val;

        L5500:
            if ((game.Objects.oflag2[(int)ObjectIndices.bottl - 1] & ObjectFlags2.OPENBT) != 0)
            {
                goto L5550;
            }
            /* 						!BOTTLE OPEN? */
            MessageHandler.Speak(612, game);
            /* 						!NO, LOSE. */
            return ret_val;

        L5550:
            if (ObjectHandler.IsObjectEmpty(ObjectIndices.bottl, game))
            {
                goto L5600;
            }
            /* 						!OPEN, EMPTY? */
            MessageHandler.Speak(613, game);
            /* 						!NO, ALREADY FULL. */
            return ret_val;

        L5600:
            newsta_((int)ObjectIndices.water, 614, 0, (int)ObjectIndices.bottl, 0, game);
            /* 						!TAKE WATER TO BOTTLE. */
            return ret_val;

        L5700:
            if (game.ParserVectors.prsa != (int)VIndices.dropw
                && game.ParserVectors.prsa != (int)VIndices.pourw
                && game.ParserVectors.prsa != (int)VIndices.givew)
            {
                goto L5900;
            }

            if (av != 0)
            {
                goto L5800;
            }

            /* 						!INTO VEHICLE? */
            newsta_(game.ParserVectors.prso, 133, 0, 0, 0, game);
            /* 						!NO, VANISHES. */
            return ret_val;

        L5800:
            newsta_(ObjectIndices.water, 0, 0, av, 0, game);
            /* 						!WATER INTO VEHICLE. */
            MessageHandler.rspsub_(296, game.Objects.odesc2[av - 1], game);
            /* 						!DESCRIBE. */
            return ret_val;

        L5900:
            if (game.ParserVectors.prsa != (int)VIndices.throww)
            {
                goto L10;
            }
            /* 						!LAST CHANCE, THROW? */
            newsta_(game.ParserVectors.prso, 132, 0, 0, 0, game);
            /* 						!VANISHES. */
            return ret_val;
        /* OAPPLI, PAGE 4 */

        /* O102--	LEAF PILE */

        L10000:
            if (game.ParserVectors.prsa != (int)VIndices.burnw)
            {
                goto L10500;
            }
            /* 						!BURN? */
            if (game.Objects.oroom[game.ParserVectors.prso - 1] == 0)
            {
                goto L10100;
            }
            /* 						!WAS HE CARRYING? */
            newsta_(game.ParserVectors.prso, 158, 0, 0, 0, game);
            /* 						!NO, BURN IT. */
            return ret_val;

        L10100:
            newsta_(game.ParserVectors.prso, 0, game.Player.Here, 0, 0, game);
            /* 						!DROP LEAVES. */
            AdventurerHandler.jigsup_(game, 159);
            /* 						!BURN HIM. */
            return ret_val;

        L10500:
            if (game.ParserVectors.prsa != (int)VIndices.movew)
            {
                goto L10600;
            }
            /* 						!MOVE? */
            MessageHandler.Speak(2, game);
            /* 						!DONE. */
            return ret_val;

        L10600:
            if (game.ParserVectors.prsa != (int)VIndices.lookuw || game.Switches.rvclr != 0)
            {
                goto L10;
            }
            MessageHandler.Speak(344, game);
            /* 						!LOOK UNDER? */
            return ret_val;

        /* O103--	TROLL, DONE EXTERNALLY. */

        L11000:
            ret_val = villns.trollp_(game, arg);
            /* 						!TROLL PROCESSOR. */
            return ret_val;

        /* O104--	RUSTY KNIFE. */

        L12000:
            if (game.ParserVectors.prsa != (int)VIndices.takew)
            {
                goto L12100;
            }
            /* 						!TAKE? */
            if (game.Objects.oadv[(int)ObjectIndices.sword - 1] == game.Player.Winner)
            {
                MessageHandler.Speak(160, game);
            }
            /* 						!PULSE SWORD. */
            goto L10;

        L12100:
            if ((game.ParserVectors.prsa != (int)VIndices.attacw && game.ParserVectors.prsa != (int)VIndices.killw
                || game.ParserVectors.prsi != (int)ObjectIndices.rknif)
                && (game.ParserVectors.prsa != (int)VIndices.swingw && game.ParserVectors.prsa != (int)VIndices.throww || game.ParserVectors.prso != (int)ObjectIndices.rknif))
            {
                goto L10;
            }
            newsta_(ObjectIndices.rknif, 0, 0, 0, 0, game);
            /* 						!KILL KNIFE. */
            AdventurerHandler.jigsup_(game, 161);
            /* 						!KILL HIM. */
            return ret_val;
        /* OAPPLI, PAGE 5 */

        /* O105--	GLACIER */

        L15000:
            if (game.ParserVectors.prsa != (int)VIndices.throww)
            {
                goto L15500;
            }
            /* 						!THROW? */
            if (game.ParserVectors.prso != (int)ObjectIndices.torch)
            {
                goto L15400;
            }
            /* 						!TORCH? */
            newsta_((int)ObjectIndices.ice, 169, 0, 0, 0, game);
            /* 						!MELT ICE. */
            game.Objects.odesc1[(int)ObjectIndices.torch - 1] = 174;
            /* 						!MUNG TORCH. */
            game.Objects.odesc2[(int)ObjectIndices.torch - 1] = 173;
            game.Objects.oflag1[(int)ObjectIndices.torch - 1] &= ~(ObjectFlags)flobts;
            newsta_((int)ObjectIndices.torch, 0, (int)RoomIndices.strea, 0, 0, game);
            /* 						!MOVE TORCH. */
            game.Flags.glacrf = true;
            /* 						!GLACIER GONE. */
            if (!RoomHandler.IsRoomLit(game.Player.Here, game))
            {
                MessageHandler.Speak(170, game);
            }
            /* 						!IN DARK? */
            return ret_val;

        L15400:
            MessageHandler.Speak(171, game);
            /* 						!JOKE IF NOT TORCH. */
            return ret_val;

        L15500:
            if (game.ParserVectors.prsa != (int)VIndices.meltw || game.ParserVectors.prso != (int)ObjectIndices.ice)
            {
                goto L10;
            }

            if ((game.Objects.oflag1[game.ParserVectors.prsi - 1] & (ObjectFlags)flobts) == (ObjectFlags)flobts)
            {
                goto L15600;
            }

            MessageHandler.rspsub_(298, odi2, game);
            /* 						!CANT MELT WITH THAT. */
            return ret_val;

        L15600:
            game.Flags.glacmf = true;
            /* 						!PARTIAL MELT. */
            if (game.ParserVectors.prsi != (int)ObjectIndices.torch)
            {
                goto L15700;
            }
            /* 						!MELT WITH TORCH? */
            game.Objects.odesc1[(int)ObjectIndices.torch - 1] = 174;
            /* 						!MUNG TORCH. */
            game.Objects.odesc2[(int)ObjectIndices.torch - 1] = 173;
            game.Objects.oflag1[(int)ObjectIndices.torch - 1] &= ~(ObjectFlags)flobts;

        L15700:
            AdventurerHandler.jigsup_(game, 172);
            /* 						!DROWN. */
            return ret_val;

        /* O106--	BLACK BOOK */

        L18000:
            if (game.ParserVectors.prsa != (int)VIndices.openw)
            {
                goto L18100;
            }
            /* 						!OPEN? */
            MessageHandler.Speak(180, game);
            /* 						!JOKE. */
            return ret_val;

        L18100:
            if (game.ParserVectors.prsa != (int)VIndices.closew)
            {
                goto L18200;
            }
            /* 						!CLOSE? */
            MessageHandler.Speak(181, game);
            return ret_val;

        L18200:
            if (game.ParserVectors.prsa != (int)VIndices.burnw)
            {
                goto L10;
            }
            /* 						!BURN? */
            newsta_(game.ParserVectors.prso, 0, 0, 0, 0, game);
            /* 						!FATAL JOKE. */
            AdventurerHandler.jigsup_(game, 182);
            return ret_val;
        /* OAPPLI, PAGE 6 */

        /* O107--	CANDLES, PROCESSED EXTERNALLY */

        L19000:
            ret_val = LightHandler.lightp_(game, ObjectIndices.candl);
            return ret_val;

        /* O108--	MATCHES, PROCESSED EXTERNALLY */

        L20000:
            ret_val = LightHandler.lightp_(game, ObjectIndices.match);
            return ret_val;

        /* O109--	CYCLOPS, PROCESSED EXTERNALLY. */

        L22000:
            ret_val = villns.cyclop_(game, arg);
            /* 						!CYCLOPS */
            return ret_val;

        /* O110--	THIEF, PROCESSED EXTERNALLY */

        L25000:
            ret_val = villns.thiefp_(game, arg);
            return ret_val;

        /* O111--	WINDOW */

        L26000:
            ret_val = RoomHandler.opncls_((int)ObjectIndices.windo, 208, 209, game);
            /* 						!OPEN/CLS WINDOW. */
            return ret_val;

        /* O112--	PILE OF BODIES */

        L32000:
            if (game.ParserVectors.prsa != (int)VIndices.takew)
            {
                goto L32500;
            }
            /* 						!TAKE? */
            MessageHandler.Speak(228, game);
            /* 						!CANT. */
            return ret_val;

        L32500:
            if (game.ParserVectors.prsa != (int)VIndices.burnw && game.ParserVectors.prsa != (int)VIndices.mungw)
            {
                goto L10;
            }
            if (game.Flags.onpolf)
            {
                return ret_val;
            }
            /* 						!BURN OR MUNG? */
            game.Flags.onpolf = true;
            /* 						!SET HEAD ON POLE. */
            newsta_(ObjectIndices.hpole, 0, RoomIndices.lld2, 0, 0, game);
            AdventurerHandler.jigsup_(game, 229);
            /* 						!BEHEADED. */
            return ret_val;

        /* O113--	VAMPIRE BAT */

        L35000:
            MessageHandler.Speak(50, game);
            /* 						!TIME TO FLY, JACK. */
            f = AdventurerHandler.moveto_(game, bats.batdrp[game.rnd_(9)], game.Player.Winner);
            /* 						!SELECT RANDOM DEST. */
            f = RoomHandler.RoomDescription(0, game);
            return ret_val;
        /* OAPPLI, PAGE 7 */

        /* O114--	STICK */

        L39000:
            if (game.ParserVectors.prsa != (int)VIndices.wavew)
            {
                goto L10;
            }
            /* 						!WAVE? */
            if (game.Player.Here == (int)RoomIndices.mrain)
            {
                goto L39500;
            }
            /* 						!ON RAINBOW? */
            if (game.Player.Here == (int)RoomIndices.pog || game.Player.Here == (int)RoomIndices.falls)
            {
                goto L39200;
            }
            MessageHandler.Speak(244, game);
            /* 						!NOTHING HAPPENS. */
            return ret_val;

        L39200:
            game.Objects.oflag1[(int)ObjectIndices.pot - 1] |= ObjectFlags.VISIBT;
            game.Flags.rainbf = !game.Flags.rainbf;
            /* 						!COMPLEMENT RAINBOW. */
            i = 245;
            /* 						!ASSUME OFF. */
            if (game.Flags.rainbf)
            {
                i = 246;
            }
            /* 						!IF ON, SOLID. */
            MessageHandler.Speak(i, game);
            /* 						!DESCRIBE. */
            return ret_val;

        L39500:
            game.Flags.rainbf = false;
            /* 						!ON RAINBOW, */
            AdventurerHandler.jigsup_(game, 247);
            /* 						!TAKE A FALL. */
            return ret_val;

        /* O115--	BALLOON, HANDLED EXTERNALLY */

        L40000:
            ret_val = BalloonHandler.ballop_(game, arg);
            return ret_val;

        /* O116--	HEADS */

        L45000:
            if (game.ParserVectors.prsa != (int)VIndices.hellow)
            {
                goto L45100;
            }
            /* 						!HELLO HEADS? */
            MessageHandler.Speak(633, game);
            /* 						!TRULY BIZARRE. */
            return ret_val;

        L45100:
            if (game.ParserVectors.prsa == (int)VIndices.readw)
            {
                goto L10;
            }
            /* 						!READ IS OK. */
            newsta_(ObjectIndices.lcase, 260, RoomIndices.lroom, 0, 0, game);
            /* 						!MAKE LARGE CASE. */
            i = dso4.robadv_(game, game.Player.Winner, 0, ObjectIndices.lcase, 0) + dso4.robrm_(game, game.Player.Here, 100, 0, (int)ObjectIndices.lcase, 0);
            AdventurerHandler.jigsup_(game, 261);
            /* 						!KILL HIM. */
            return ret_val;
        /* OAPPLI, PAGE 8 */

        /* O117--	SPHERE */

        L47000:
            if (game.Flags.cagesf || game.ParserVectors.prsa != (int)VIndices.takew)
            {
                goto L10;
            }
            /* 						!TAKE? */
            if (game.Player.Winner != (int)AIndices.player)
            {
                goto L47500;
            }
            /* 						!ROBOT TAKE? */
            MessageHandler.Speak(263, game);
            /* 						!NO, DROP CAGE. */
            if (game.Objects.oroom[(int)ObjectIndices.robot - 1] != game.Player.Here)
            {
                goto L47200;
            }
            /* 						!ROBOT HERE? */
            f = AdventurerHandler.moveto_(game, RoomIndices.caged, game.Player.Winner);
            /* 						!YES, MOVE INTO CAGE. */
            newsta_(ObjectIndices.robot, 0, RoomIndices.caged, 0, 0, game);
            /* 						!MOVE ROBOT. */
            game.Adventurers.Rooms[(int)AIndices.arobot - 1] = (int)RoomIndices.caged;
            game.Objects.oflag1[(int)ObjectIndices.robot - 1] |= ObjectFlags.NDSCBT;
            game.Clock.Ticks[(int)ClockIndices.cevsph - 1] = 10;
            /* 						!GET OUT IN 10 OR ELSE. */
            return ret_val;

        L47200:
            newsta_(ObjectIndices.spher, 0, 0, 0, 0, game);
            /* 						!YOURE DEAD. */
            game.Rooms.RoomFlags[(int)RoomIndices.cager - 1] |= RoomFlags.RMUNG;
            game.Rooms.RoomActions[(int)RoomIndices.cager - 1] = 147;
            AdventurerHandler.jigsup_(game, 148);
            /* 						!MUNG PLAYER. */
            return ret_val;

        L47500:
            newsta_(ObjectIndices.spher, 0, 0, 0, 0, game);
            /* 						!ROBOT TRIED, */
            newsta_(ObjectIndices.robot, 264, 0, 0, 0, game);
            /* 						!KILL HIM. */
            newsta_(ObjectIndices.cage, 0, game.Player.Here, 0, 0, game);
            /* 						!INSERT MANGLED CAGE. */
            return ret_val;

        /* O118--	GEOMETRICAL BUTTONS */

        L48000:
            if (game.ParserVectors.prsa != (int)VIndices.pushw)
            {
                goto L10;
            }
            /* 						!PUSH? */
            i = game.ParserVectors.prso - (int)ObjectIndices.sqbut + 1;
            /* 						!GET BUTTON INDEX. */
            if (i <= 0 || i >= 4)
            {
                goto L10;
            }
            /* 						!A BUTTON? */
            if (game.Player.Winner != (int)AIndices.player)
            {
                switch (i)
                {
                    case 1: goto L48100;
                    case 2: goto L48200;
                    case 3: goto L48300;
                }
            }
            AdventurerHandler.jigsup_(game, 265);
            /* 						!YOU PUSHED, YOU DIE. */
            return ret_val;

        L48100:
            i = 267;
            if (game.Flags.carozf)
            {
                i = 266;
            }
            /* 						!SPEED UP? */
            game.Flags.carozf = true;
            MessageHandler.Speak(i, game);
            return ret_val;

        L48200:
            i = 266;
            /* 						!ASSUME NO CHANGE. */
            if (game.Flags.carozf)
            {
                i = 268;
            }
            game.Flags.carozf = false;
            MessageHandler.Speak(i, game);
            return ret_val;

        L48300:
            game.Flags.caroff = !game.Flags.caroff;
            /* 						!FLIP CAROUSEL. */
            if (!qhere_((int)ObjectIndices.irbox, (int)RoomIndices.carou, game))
            {
                return ret_val;
            }
            /* 						!IRON BOX IN CAROUSEL? */
            MessageHandler.Speak(269, game);
            /* 						!YES, THUMP. */
            game.Objects.oflag1[(int)ObjectIndices.irbox - 1] ^= ObjectFlags.VISIBT;
            if (game.Flags.caroff)
            {
                game.Rooms.RoomFlags[(int)RoomIndices.carou - 1] &= ~RoomFlags.RSEEN;
            }
            return ret_val;

        /* O119--	FLASK FUNCTION */

        L49000:
            if (game.ParserVectors.prsa == (int)VIndices.openw)
            {
                goto L49100;
            }
            /* 						!OPEN? */
            if (game.ParserVectors.prsa != (int)VIndices.mungw && game.ParserVectors.prsa != (int)VIndices.throww)
            {
                goto L10;
            }
            newsta_(ObjectIndices.flask, 270, 0, 0, 0, game);
        /* 						!KILL FLASK. */
        L49100:
            game.Rooms.RoomFlags[game.Player.Here - 1] |= RoomFlags.RMUNG;
            game.Rooms.RoomActions[game.Player.Here - 1] = 271;
            AdventurerHandler.jigsup_(game, 272);
            /* 						!POISONED. */
            return ret_val;

        /* O120--	BUCKET FUNCTION */

        L50000:
            if (arg != 2)
            {
                goto L10;
            }

            /* 						!READOUT? */
            if (game.Objects.ocan[(int)ObjectIndices.water - 1] != (int)ObjectIndices.bucke || game.Flags.bucktf)
            {
                goto L50500;
            }

            game.Flags.bucktf = true;
            /* 						!BUCKET AT TOP. */
            game.Clock.Ticks[(int)ClockIndices.cevbuc - 1] = 100;
            /* 						!START COUNTDOWN. */
            newsta_(ObjectIndices.bucke, 290, RoomIndices.twell, 0, 0, game);
            /* 						!REPOSITION BUCKET. */
            goto L50900;
        /* 						!FINISH UP. */

        L50500:
            if (game.Objects.ocan[(int)ObjectIndices.water - 1] == (int)ObjectIndices.bucke || !game.Flags.bucktf)
            {
                goto L10;
            }

            game.Flags.bucktf = false;
            newsta_(ObjectIndices.bucke, 291, RoomIndices.bwell, 0, 0, game);
        /* 						!BUCKET AT BOTTOM. */
        L50900:
            if (av != (int)ObjectIndices.bucke)
            {
                return ret_val;
            }
            /* 						!IN BUCKET? */
            f = AdventurerHandler.moveto_(game, game.Objects.oroom[(int)ObjectIndices.bucke - 1], game.Player.Winner);
            /* 						!MOVE ADVENTURER. */
            f = RoomHandler.RoomDescription(0, game);
            /* 						!DESCRIBE ROOM. */
            return ret_val;
        /* OAPPLI, PAGE 9 */

        /* O121--	EATME CAKE */

        L51000:
            if (game.ParserVectors.prsa != (int)VIndices.eatw || game.ParserVectors.prso != (int)ObjectIndices.ecake || game.Player.Here != (int)RoomIndices.alice)
            {
                goto L10;
            }
            newsta_(ObjectIndices.ecake, 273, 0, 0, 0, game);
            /* 						!VANISH CAKE. */
            game.Objects.oflag1[(int)ObjectIndices.robot - 1] &= ~ObjectFlags.VISIBT;
            ret_val = AdventurerHandler.moveto_(game, RoomIndices.alism, game.Player.Winner);
            /* 						!MOVE TO ALICE SMALL. */
            iz = 64;
            ir = (int)RoomIndices.alism;
            io = (int)RoomIndices.alice;
            goto L52405;

        /* O122--	ICINGS */

        L52000:
            if (game.ParserVectors.prsa != (int)VIndices.readw)
            {
                goto L52200;
            }
            /* 						!READ? */
            i = 274;
            /* 						!CANT READ. */
            if (game.ParserVectors.prsi != 0)
            {
                i = 275;
            }
            /* 						!THROUGH SOMETHING? */
            if (game.ParserVectors.prsi == (int)ObjectIndices.bottl)
            {
                i = 276;
            }

            /* 						!THROUGH BOTTLE? */
            if (game.ParserVectors.prsi == (int)ObjectIndices.flask)
            {
                i = game.ParserVectors.prso - (int)ObjectIndices.orice + 277;
            }

            /* 						!THROUGH FLASK? */
            MessageHandler.Speak(i, game);
            /* 						!READ FLASK. */
            return ret_val;

        L52200:
            if (game.ParserVectors.prsa != (int)VIndices.throww || game.ParserVectors.prso != (int)ObjectIndices.rdice ||
                 game.ParserVectors.prsi != (int)ObjectIndices.pool)
            {
                goto L52300;
            }

            newsta_(ObjectIndices.pool, 280, 0, 0, 0, game);
            /* 						!VANISH POOL. */
            game.Objects.oflag1[(int)ObjectIndices.saffr - 1] |= ObjectFlags.VISIBT;
            return ret_val;

        L52300:
            if (game.Player.Here != (int)RoomIndices.alice && game.Player.Here != (int)RoomIndices.alism &&
                game.Player.Here != (int)RoomIndices.alitr)
            {
                goto L10;
            }
            if (game.ParserVectors.prsa != (int)VIndices.eatw
                && game.ParserVectors.prsa != (int)VIndices.throww
                || game.ParserVectors.prso != (int)ObjectIndices.orice)
            {
                goto L52400;
            }

            newsta_(ObjectIndices.orice, 0, 0, 0, 0, game);
            /* 						!VANISH ORANGE ICE. */
            game.Rooms.RoomFlags[game.Player.Here - 1] |= RoomFlags.RMUNG;
            game.Rooms.RoomActions[game.Player.Here - 1] = 281;
            AdventurerHandler.jigsup_(game, 282);
            /* 						!VANISH ADVENTURER. */
            return ret_val;

        L52400:
            if (game.ParserVectors.prsa != (int)VIndices.eatw || game.ParserVectors.prso != (int)ObjectIndices.blice)
            {
                goto L10;
            }
            newsta_(ObjectIndices.blice, 283, 0, 0, 0, game);
            /* 						!VANISH BLUE ICE. */
            if (game.Player.Here != (int)RoomIndices.alism)
            {
                goto L52500;
            }
            /* 						!IN REDUCED ROOM? */
            game.Objects.oflag1[(int)ObjectIndices.robot - 1] |= ObjectFlags.VISIBT;
            io = game.Player.Here;
            ret_val = AdventurerHandler.moveto_(game, RoomIndices.alice, game.Player.Winner);
            iz = 0;
            ir = (int)RoomIndices.alice;

        /*  Do a size change, common loop used also by code at 51000 */

        L52405:
            i__1 = game.Objects.Count;
            for (i = 1; i <= i__1; ++i)
            {
                /* 						!ENLARGE WORLD. */
                if (game.Objects.oroom[i - 1] != io || game.Objects.osize[i - 1] == 10000)
                {
                    goto L52450;
                }
                game.Objects.oroom[i - 1] = ir;
                game.Objects.osize[i - 1] *= iz;
            L52450:
                ;
            }
            return ret_val;

        L52500:
            AdventurerHandler.jigsup_(game, 284);
            /* 						!ENLARGED IN WRONG ROOM. */
            return ret_val;

        /* O123--	BRICK */

        L54000:
            if (game.ParserVectors.prsa != (int)VIndices.burnw)
            {
                goto L10;
            }
            /* 						!BURN? */
            AdventurerHandler.jigsup_(game, 150);
            /* 						!BOOM */
            /* 						! */
            return ret_val;

        /* O124--	MYSELF */

        L55000:
            if (game.ParserVectors.prsa != (int)VIndices.givew)
            {
                goto L55100;
            }
            /* 						!GIVE? */
            newsta_(game.ParserVectors.prso, 2, 0, 0, (int)AIndices.player, game);
            /* 						!DONE. */
            return ret_val;

        L55100:
            if (game.ParserVectors.prsa != (int)VIndices.takew)
            {
                goto L55200;
            }
            /* 						!TAKE? */
            MessageHandler.Speak(286, game);
            /* 						!JOKE. */
            return ret_val;

        L55200:
            if (game.ParserVectors.prsa != (int)VIndices.killw && game.ParserVectors.prsa != (int)VIndices.mungw)
            {
                goto L10;
            }
            AdventurerHandler.jigsup_(game, 287);
            /* 						!KILL, NO JOKE. */
            return ret_val;
        /* OAPPLI, PAGE 10 */

        /* O125--	PANELS INSIDE MIRROR */

        L56000:
            if (game.ParserVectors.prsa != (int)VIndices.pushw)
            {
                goto L10;
            }
            /* 						!PUSH? */
            if (game.Switches.poleuf != 0)
            {
                goto L56100;
            }
            /* 						!SHORT POLE UP? */
            i = 731;
            /* 						!NO, WONT BUDGE. */
            if (game.Switches.mdir % 180 == 0)
            {
                i = 732;
            }
            /* 						!DIFF MSG IF N-S. */
            MessageHandler.Speak(i, game);
            /* 						!TELL WONT MOVE. */
            return ret_val;

        L56100:
            if (game.Switches.mloc != (int)RoomIndices.mrg)
            {
                goto L56200;
            }
            /* 						!IN GDN ROOM? */
            MessageHandler.Speak(733, game);
            /* 						!YOU LOSE. */
            AdventurerHandler.jigsup_(game, 685);
            return ret_val;

        L56200:
            i = 831;
            /* 						!ROTATE L OR R. */
            if (game.ParserVectors.prso == (int)ObjectIndices.rdwal || game.ParserVectors.prso == (int)ObjectIndices.ylwal)
            {
                i = 830;
            }
            MessageHandler.Speak(i, game);
            /* 						!TELL DIRECTION. */
            game.Switches.mdir = (game.Switches.mdir + 45 + (i - 830) * 270) % 360;
            /* 						!CALCULATE NEW DIR. */
            i__1 = game.Switches.mdir / 45 + 695;
            MessageHandler.rspsub_(734, i__1, game);
            /* 						!TELL NEW DIR. */
            if (game.Flags.wdopnf)
            {
                MessageHandler.Speak(730, game);
            }
            /* 						!IF PANEL OPEN, CLOSE. */
            game.Flags.wdopnf = false;
            return ret_val;
        /* 						!DONE. */

        /* O126--	ENDS INSIDE MIRROR */

        L57000:
            if (game.ParserVectors.prsa != (int)VIndices.pushw)
            {
                goto L10;
            }
            /* 						!PUSH? */
            if (game.Switches.mdir % 180 == 0)
            {
                goto L57100;
            }
            /* 						!MIRROR N-S? */
            MessageHandler.Speak(735, game);
            /* 						!NO, WONT BUDGE. */
            return ret_val;

        L57100:
            if (game.ParserVectors.prso != (int)ObjectIndices.pindr)
            {
                goto L57300;
            }
            /* 						!PUSH PINE WALL? */
            if (game.Switches.mloc == (int)RoomIndices.mrc && game.Switches.mdir == 180 ||
                game.Switches.mloc == (int)RoomIndices.mrd && game.Switches.mdir == 0 ||
                game.Switches.mloc == (int)RoomIndices.mrg)
            {
                goto L57200;
            }
            MessageHandler.Speak(736, game);
            /* 						!NO, OPENS. */
            game.Flags.wdopnf = true;
            /* 						!INDICATE OPEN. */
            game.Clock.Flags[(int)ClockIndices.cevpin - 1] = true;
            /* 						!TIME OPENING. */

            game.Clock.Ticks[(int)ClockIndices.cevpin - 1] = 5;
            return ret_val;

        L57200:
            MessageHandler.Speak(737, game);
            /* 						!GDN SEES YOU, DIE. */
            AdventurerHandler.jigsup_(game, 685);
            return ret_val;

        L57300:
            nloc = game.Switches.mloc - 1;
            /* 						!NEW LOC IF SOUTH. */
            if (game.Switches.mdir == 0)
            {
                nloc = game.Switches.mloc + 1;
            }
            /* 						!NEW LOC IF NORTH. */
            if (nloc >= (int)RoomIndices.mra && nloc <= (int)RoomIndices.mrd)
            {
                goto L57400;
            }
            MessageHandler.Speak(738, game);
            /* 						!HAVE REACHED END. */
            return ret_val;

        L57400:
            i = 699;
            /* 						!ASSUME SOUTH. */
            if (game.Switches.mdir == 0)
            {
                i = 695;
            }
            /* 						!NORTH. */
            j = 739;
            /* 						!ASSUME SMOOTH. */
            if (game.Switches.poleuf != 0)
            {
                j = 740;
            }
            /* 						!POLE UP, WOBBLES. */
            MessageHandler.rspsub_(j, i, game);
            /* 						!DESCRIBE. */
            game.Switches.mloc = nloc;
            if (game.Switches.mloc != (int)RoomIndices.mrg)
            {
                return ret_val;
            }
            /* 						!NOW IN GDN ROOM? */

            if (game.Switches.poleuf != 0)
            {
                goto L57500;
            }
            /* 						!POLE UP, GDN SEES. */
            if (game.Flags.mropnf || game.Flags.wdopnf)
            {
                goto L57600;
            }
            /* 						!DOOR OPEN, GDN SEES. */
            if (game.Flags.mr1f && game.Flags.mr2f)
            {
                return ret_val;
            }
            /* 						!MIRRORS INTACT, OK. */
            MessageHandler.Speak(742, game);
            /* 						!MIRRORS BROKEN, DIE. */
            AdventurerHandler.jigsup_(game, 743);
            return ret_val;

        L57500:
            MessageHandler.Speak(741, game);
            /* 						!POLE UP, DIE. */
            AdventurerHandler.jigsup_(game, 743);
            return ret_val;

        L57600:
            MessageHandler.Speak(744, game);
            /* 						!DOOR OPEN, DIE. */
            AdventurerHandler.jigsup_(game, 743);
            return ret_val;
        /* OAPPLI, PAGE 11 */

        /* O127--	GLOBAL GUARDIANS */

        L58000:
            if (game.ParserVectors.prsa != (int)VIndices.attacw && game.ParserVectors.prsa != (int)VIndices.killw && game.ParserVectors.prsa != (int)VIndices.mungw)
            {
                goto L58100;
            }

            AdventurerHandler.jigsup_(game, 745);
            /* 						!LOSE. */
            return ret_val;

        L58100:
            if (game.ParserVectors.prsa != (int)VIndices.hellow)
            {
                goto L10;
            }
            /* 						!HELLO? */
            MessageHandler.Speak(746, game);
            /* 						!NO REPLY. */
            return ret_val;

        /* O128--	GLOBAL MASTER */

        L59000:
            if (game.ParserVectors.prsa != (int)VIndices.attacw && game.ParserVectors.prsa != (int)VIndices.killw && game.ParserVectors.prsa != (int)VIndices.mungw)
            {
                goto L59100;
            }
            AdventurerHandler.jigsup_(game, 747);
            /* 						!BAD IDEA. */
            return ret_val;

        L59100:
            if (game.ParserVectors.prsa != (int)VIndices.takew)
            {
                goto L10;
            }
            /* 						!TAKE? */
            MessageHandler.Speak(748, game);
            /* 						!JOKE. */
            return ret_val;

        /* O129--	NUMERAL FIVE (FOR JOKE) */

        L60000:
            if (game.ParserVectors.prsa != (int)VIndices.takew)
            {
                goto L10;
            }
            /* 						!TAKE FIVE? */
            MessageHandler.Speak(419, game);
            /* 						!TIME PASSES. */
            for (i = 1; i <= 3; ++i)
            {
                /* 						!WAIT A WHILE. */
                if (ClockEvents.clockd_(game))
                {
                    return ret_val;
                }
                /* L60100: */
            }
            return ret_val;

        /* O130--	CRYPT FUNCTION */

        L61000:
            if (!game.Flags.endgmf)
            {
                goto L45000;
            }
            /* 						!IF NOT EG, DIE. */
            if (game.ParserVectors.prsa != (int)VIndices.openw)
            {
                goto L61100;
            }
            /* 						!OPEN? */
            i = 793;
            if ((game.Objects.oflag2[(int)ObjectIndices.tomb - 1] & ObjectFlags2.OPENBT) != 0)
            {
                i = 794;
            }
            MessageHandler.Speak(i, game);
            game.Objects.oflag2[(int)ObjectIndices.tomb - 1] |= ObjectFlags2.OPENBT;
            return ret_val;

        L61100:
            if (game.ParserVectors.prsa != (int)VIndices.closew)
            {
                goto L45000;
            }
            /* 						!CLOSE? */
            i = 795;
            if ((game.Objects.oflag2[(int)ObjectIndices.tomb - 1] & ObjectFlags2.OPENBT) != 0)
            {
                i = 796;
            }
            MessageHandler.Speak(i, game);
            game.Objects.oflag2[(int)ObjectIndices.tomb - 1] &= ~ObjectFlags2.OPENBT;
            if (game.Player.Here == (int)RoomIndices.crypt)
            {
                game.Clock.Ticks[(int)ClockIndices.cevste - 1] = 3;
            }
            /* 						!IF IN CRYPT, START EG. */
            return ret_val;
        /* OAPPLI, PAGE 12 */

        /* O131--	GLOBAL LADDER */

        L62000:
            if (PuzzleHandler.cpvec[game.Switches.cphere] == -2 || PuzzleHandler.cpvec[game.Switches.cphere - 2] == -3)
            {
                goto L62100;
            }
            MessageHandler.Speak(865, game);
            /* 						!NO, LOSE. */
            return ret_val;

        L62100:
            if (game.ParserVectors.prsa == (int)VIndices.clmbw || game.ParserVectors.prsa == (int)VIndices.clmbuw)
            {
                goto L62200;
            }
            MessageHandler.Speak(866, game);
            /* 						!CLIMB IT? */
            return ret_val;

        L62200:
            if (game.Switches.cphere == 10 && PuzzleHandler.cpvec[game.Switches.cphere] == -2)
            {
                goto L62300;
            }

            MessageHandler.Speak(867, game);
            /* 						!NO, HIT YOUR HEAD. */
            return ret_val;

        L62300:
            f = AdventurerHandler.moveto_(game, RoomIndices.cpant, game.Player.Winner);
            /* 						!TO ANTEROOM. */
            f = RoomHandler.RoomDescription(3, game);
            /* 						!DESCRIBE. */
            return ret_val;
        }

        public static bool sobjs_(Game game, int ri, int arg)
        {
            int i__1;
            bool ret_val;

            bool f;
            int i;
            int mroom;
            int av;
            int odi2 = 0, odo2 = 0;

            if (game.ParserVectors.prso > 220)
            {
                goto L5;
            }

            if (game.ParserVectors.prso != 0)
            {
                odo2 = game.Objects.odesc2[game.ParserVectors.prso - 1];
            }
        L5:
            if (game.ParserVectors.prsi != 0)
            {
                odi2 = game.Objects.odesc2[game.ParserVectors.prsi - 1];
            }

            av = game.Adventurers.Vehicles[game.Player.Winner - 1];
            ret_val = true;

            switch (ri)
            {
                case 1: goto L1000;
                case 2: goto L3000;
                case 3: goto L4000;
                case 4: goto L6000;
                case 5: goto L7000;
                case 6: goto L8000;
                case 7: goto L9000;
                case 8: goto L13000;
                case 9: goto L14000;
                case 10: goto L16000;
                case 11: goto L17000;
                case 12: goto L21000;
                case 13: goto L23000;
                case 14: goto L24000;
                case 15: goto L27000;
                case 16: goto L28000;
                case 17: goto L29000;
                case 18: goto L30000;
                case 19: goto L31000;
                case 20: goto L33000;
                case 21: goto L34000;
                case 22: goto L36000;
                case 23: goto L37000;
                case 24: goto L38000;
                case 25: goto L41000;
                case 26: goto L42000;
                case 27: goto L43000;
                case 28: goto L44000;
                case 29: goto L46000;
                case 30: goto L53000;
                case 31: goto L56000;
            }
            throw new InvalidOperationException();
        //bug_(6, ri);

        /* RETURN HERE TO DECLARE FALSE RESULT */

        L10:
            ret_val = false;
            return ret_val;
        /* SOBJS, PAGE 3 */

        /* O1--	GUNK FUNCTION */

        L1000:
            if (game.Objects.ocan[(int)ObjectIndices.gunk - 1] == 0)
            {
                goto L10;
            }
            /* 						!NOT INSIDE? F */
            newsta_((int)ObjectIndices.gunk, 122, 0, 0, 0, game);
            /* 						!FALLS APART. */
            return ret_val;

        /* O2--	TROPHY CASE */

        L3000:
            if (game.ParserVectors.prsa != (int)VIndices.takew)
            {
                goto L10;
            }
            /* 						!TAKE? */
            MessageHandler.Speak(128, game);
            /* 						!CANT. */
            return ret_val;

        /* O3--	BOTTLE FUNCTION */

        L4000:
            if (game.ParserVectors.prsa != (int)VIndices.throww)
            {
                goto L4100;
            }
            /* 						!THROW? */
            newsta_(game.ParserVectors.prso, 129, 0, 0, 0, game);
            /* 						!BREAKS. */
            return ret_val;

        L4100:
            if (game.ParserVectors.prsa != (int)VIndices.mungw)
            {
                goto L10;
            }
            /* 						!MUNG? */
            newsta_(game.ParserVectors.prso, 131, 0, 0, 0, game);
            /* 						!BREAKS. */
            return ret_val;
        /* SOBJS, PAGE 4 */

        /* O4--	ROPE FUNCTION */

        L6000:
            if (game.Player.Here == (int)RoomIndices.dome)
            {
                goto L6100;
            }
            /* 						!IN DOME? */
            game.Flags.domef = false;
            /* 						!NO, */
            if (game.ParserVectors.prsa != (int)VIndices.untiew)
            {
                goto L6050;
            }
            /* 						!UNTIE? */
            MessageHandler.Speak(134, game);
            /* 						!CANT */
            return ret_val;

        L6050:
            if (game.ParserVectors.prsa != (int)VIndices.tiew)
            {
                goto L10;
            }
            /* 						!TIE? */
            MessageHandler.Speak(135, game);
            /* 						!CANT TIE */
            return ret_val;

        L6100:
            if (game.ParserVectors.prsa != (int)VIndices.tiew || game.ParserVectors.prsi != (int)ObjectIndices.raili)
            {
                goto L6200;
            }
            if (game.Flags.domef)
            {
                goto L6150;
            }
            /* 						!ALREADY TIED? */
            game.Flags.domef = true;
            /* 						!NO, TIE IT. */
            game.Objects.oflag1[(int)ObjectIndices.rope - 1] |= ObjectFlags.NDSCBT;
            game.Objects.oflag2[(int)ObjectIndices.rope - 1] |= ObjectFlags2.CLMBBT;
            newsta_((int)ObjectIndices.rope, 137, (int)RoomIndices.dome, 0, 0, game);
            return ret_val;

        L6150:
            MessageHandler.Speak(136, game);
            /* 						!DUMMY. */
            return ret_val;

        L6200:
            if (game.ParserVectors.prsa != (int)VIndices.untiew)
            {
                goto L6300;
            }
            /* 						!UNTIE? */
            if (game.Flags.domef)
            {
                goto L6250;
            }
            /* 						!TIED? */
            MessageHandler.Speak(134, game);
            /* 						!NO, DUMMY. */
            return ret_val;

        L6250:
            game.Flags.domef = false;
            /* 						!YES, UNTIE IT. */
            game.Objects.oflag1[(int)ObjectIndices.rope - 1] &= ~ObjectFlags.NDSCBT;
            game.Objects.oflag2[(int)ObjectIndices.rope - 1] &= ~ObjectFlags2.CLMBBT;
            MessageHandler.Speak(139, game);
            return ret_val;

        L6300:
            if (game.Flags.domef || game.ParserVectors.prsa != (int)VIndices.dropw)
            {
                goto L6400;
            }
            /* 						!DROP & UNTIED? */
            newsta_((int)ObjectIndices.rope, 140, (int)RoomIndices.mtorc, 0, 0, game);
            /* 						!YES, DROP. */
            return ret_val;

        L6400:
            if (game.ParserVectors.prsa != (int)VIndices.takew || !game.Flags.domef)
            {
                goto L10;
            }
            MessageHandler.Speak(141, game);
            /* 						!TAKE & TIED. */
            return ret_val;

        /* O5--	SWORD FUNCTION */

        L7000:
            if (game.ParserVectors.prsa == (int)VIndices.takew && game.Player.Winner == (int)AIndices.player)
            {
                game.Hack.swdact = true;
            }
            goto L10;

        /* O6--	LANTERN */

        L8000:
            if (game.ParserVectors.prsa != (int)VIndices.throww)
            {
                goto L8100;
            }
            /* 						!THROW? */
            newsta_((int)ObjectIndices.lamp, 0, 0, 0, 0, game);
            /* 						!KILL LAMP, */
            newsta_((int)ObjectIndices.blamp, 142, game.Player.Here, 0, 0, game);
            /* 						!REPLACE WITH BROKEN. */
            return ret_val;

        L8100:
            if (game.ParserVectors.prsa == (int)VIndices.trnonw)
            {
                game.Clock.Flags[(int)ClockIndices.cevlnt - 1] = true;
            }
            if (game.ParserVectors.prsa == (int)VIndices.trnofw)
            {
                game.Clock.Flags[(int)ClockIndices.cevlnt - 1] = false;
            }
            goto L10;

        /* O7--	RUG FUNCTION */

        L9000:
            if (game.ParserVectors.prsa != (int)VIndices.raisew)
            {
                goto L9100;
            }
            /* 						!RAISE? */
            MessageHandler.Speak(143, game);
            /* 						!CANT */
            return ret_val;

        L9100:
            if (game.ParserVectors.prsa != (int)VIndices.takew)
            {
                goto L9200;
            }
            /* 						!TAKE? */
            MessageHandler.Speak(144, game);
            /* 						!CANT */
            return ret_val;

        L9200:
            if (game.ParserVectors.prsa != (int)VIndices.movew)
            {
                goto L9300;
            }
            /* 						!MOVE? */
            i__1 = game.Switches.orrug + 145;
            MessageHandler.Speak(i__1, game);
            game.Switches.orrug = 1;
            game.Objects.oflag1[(int)ObjectIndices.door - 1] |= ObjectFlags.VISIBT;
            return ret_val;

        L9300:
            if (game.ParserVectors.prsa != (int)VIndices.lookuw
                || game.Switches.orrug != 0
                || (game.Objects.oflag2[(int)ObjectIndices.door - 1] & ObjectFlags2.OPENBT) != 0)
            {
                goto L10;
            }

            MessageHandler.Speak(345, game);
            return ret_val;
        /* SOBJS, PAGE 5 */

        /* O8--	SKELETON */

        L13000:
            i = dso4.robrm_(game, game.Player.Here, 100, (int)RoomIndices.lld2, 0, 0) + dso4.robadv_(game, game.Player.Winner, (int)RoomIndices.lld2, 0, 0);

            if (i != 0)
            {
                MessageHandler.Speak(162, game);
            }

            /* 						!IF ROBBED, SAY SO. */
            return ret_val;

        /* O9--	MIRROR */

        L14000:
            if (game.Flags.mirrmf || game.ParserVectors.prsa != (int)VIndices.rubw)
            {
                goto L14500;
            }

            mroom = game.Player.Here ^ 1;
            i__1 = game.Objects.Count;
            for (i = 1; i <= i__1; ++i)
            {
                /* 						!INTERCHANGE OBJS. */
                if (game.Objects.oroom[i - 1] == game.Player.Here)
                {
                    game.Objects.oroom[i - 1] = -1;
                }
                if (game.Objects.oroom[i - 1] == mroom)
                {
                    game.Objects.oroom[i - 1] = game.Player.Here;
                }
                if (game.Objects.oroom[i - 1] == -1)
                {
                    game.Objects.oroom[i - 1] = mroom;
                }
                /* L14100: */
            }
            f = AdventurerHandler.moveto_(game, mroom, game.Player.Winner);
            MessageHandler.Speak(163, game);
            /* 						!SHAKE WORLD. */
            return ret_val;

        L14500:
            if (game.ParserVectors.prsa != (int)VIndices.lookw && game.ParserVectors.prsa != (int)VIndices.lookiw &&
                game.ParserVectors.prsa != (int)VIndices.examiw)
            {
                goto L14600;
            }
            i = 164;
            /* 						!MIRROR OK. */
            if (game.Flags.mirrmf)
            {
                i = 165;
            }
            /* 						!MIRROR DEAD. */
            MessageHandler.Speak(i, game);
            return ret_val;

        L14600:
            if (game.ParserVectors.prsa != (int)VIndices.takew)
            {
                goto L14700;
            }
            /* 						!TAKE? */
            MessageHandler.Speak(166, game);
            /* 						!JOKE. */
            return ret_val;

        L14700:
            if (game.ParserVectors.prsa != (int)VIndices.mungw && game.ParserVectors.prsa != (int)VIndices.throww)
            {
                goto L10;
            }
            i = 167;
            /* 						!MIRROR BREAKS. */
            if (game.Flags.mirrmf)
            {
                i = 168;
            }
            /* 						!MIRROR ALREADY BROKEN. */
            game.Flags.mirrmf = true;
            game.Flags.badlkf = true;
            MessageHandler.Speak(i, game);
            return ret_val;
        /* SOBJS, PAGE 6 */

        /* O10--	DUMBWAITER */

        L16000:
            if (game.ParserVectors.prsa != (int)VIndices.raisew)
            {
                goto L16100;
            }
            /* 						!RAISE? */
            if (game.Flags.cagetf)
            {
                goto L16400;
            }
            /* 						!ALREADY AT TOP? */
            newsta_((int)ObjectIndices.tbask, 175, (int)RoomIndices.tshaf, 0, 0, game);
            /* 						!NO, RAISE BASKET. */
            newsta_((int)ObjectIndices.fbask, 0, (int)RoomIndices.bshaf, 0, 0, game);
            game.Flags.cagetf = true;
            /* 						!AT TOP. */
            return ret_val;

        L16100:
            if (game.ParserVectors.prsa != (int)VIndices.lowerw)
            {
                goto L16200;
            }
            /* 						!LOWER? */
            if (!game.Flags.cagetf)
            {
                goto L16400;
            }
            /* 						!ALREADY AT BOTTOM? */
            newsta_((int)ObjectIndices.tbask, 176, (int)RoomIndices.bshaf, 0, 0, game);
            /* 						!NO, LOWER BASKET. */
            newsta_((int)ObjectIndices.fbask, 0, (int)RoomIndices.tshaf, 0, 0, game);
            game.Flags.cagetf = false;
            if (!RoomHandler.IsRoomLit(game.Player.Here, game))
            {
                MessageHandler.Speak(406, game);
            }
            /* 						!IF DARK, DIE. */
            return ret_val;

        L16200:
            if (game.ParserVectors.prso != (int)ObjectIndices.fbask && game.ParserVectors.prsi != (int)ObjectIndices.fbask)
            {
                goto L16300;
            }
            MessageHandler.Speak(130, game);
            /* 						!WRONG BASKET. */
            return ret_val;

        L16300:
            if (game.ParserVectors.prsa != (int)VIndices.takew)
            {
                goto L10;
            }
            /* 						!TAKE? */
            MessageHandler.Speak(177, game);
            /* 						!JOKE. */
            return ret_val;

        L16400:
            i__1 = game.rnd_(3) + 125;
            MessageHandler.Speak(i__1, game);
            /* 						!DUMMY. */
            return ret_val;

        /* O11--	GHOST FUNCTION */

        L17000:
            i = 178;
            /* 						!ASSUME DIRECT. */
            if (game.ParserVectors.prso != (int)ObjectIndices.ghost)
            {
                i = 179;
            }
            /* 						!IF NOT, INDIRECT. */
            MessageHandler.Speak(i, game);
            return ret_val;
        /* 						!SPEAK AND EXIT. */
        /* SOBJS, PAGE 7 */

        /* O12--	TUBE */

        L21000:
            if (game.ParserVectors.prsa != (int)VIndices.putw || game.ParserVectors.prsi != (int)ObjectIndices.tube)
            {
                goto L10;
            }
            MessageHandler.Speak(186, game);
            /* 						!CANT PUT BACK IN. */
            return ret_val;

        /* O13--	CHALICE */

        L23000:
            if (game.ParserVectors.prsa != (int)VIndices.takew
                || game.Objects.ocan[game.ParserVectors.prso - 1] != 0
                || game.Objects.oroom[game.ParserVectors.prso - 1] != (int)RoomIndices.treas
                || game.Objects.oroom[(int)ObjectIndices.thief - 1] != (int)RoomIndices.treas
                || (game.Objects.oflag2[(int)ObjectIndices.thief - 1] & ObjectFlags2.FITEBT) == 0
                || !game.Hack.thfact)
            {
                goto L10;
            }

            MessageHandler.Speak(204, game);
            /* 						!CANT TAKE. */
            return ret_val;

        /* O14--	PAINTING */

        L24000:
            if (game.ParserVectors.prsa != (int)VIndices.mungw)
            {
                goto L10;
            }

            /* 						!MUNG? */
            MessageHandler.Speak(205, game);
            /* 						!DESTROY PAINTING. */
            game.Objects.ofval[game.ParserVectors.prso - 1] = 0;
            game.Objects.otval[game.ParserVectors.prso - 1] = 0;
            game.Objects.odesc1[game.ParserVectors.prso - 1] = 207;
            game.Objects.odesc2[game.ParserVectors.prso - 1] = 206;
            return ret_val;
        /* SOBJS, PAGE 8 */

        /* O15--	BOLT */

        L27000:
            if (game.ParserVectors.prsa != (int)VIndices.turnw)
            {
                goto L10;
            }

            /* 						!TURN BOLT? */
            if (game.ParserVectors.prsi != (int)ObjectIndices.wrenc)
            {
                goto L27500;
            }

            /* 						!WITH WRENCH? */
            if (game.Flags.gatef)
            {
                goto L27100;
            }
            /* 						!PROPER BUTTON PUSHED? */
            MessageHandler.Speak(210, game);
            /* 						!NO, LOSE. */
            return ret_val;

        L27100:
            if (game.Flags.lwtidf)
            {
                goto L27200;
            }

            /* 						!LOW TIDE NOW? */
            game.Flags.lwtidf = true;

            /* 						!NO, EMPTY DAM. */
            MessageHandler.Speak(211, game);
            game.Objects.oflag2[(int)ObjectIndices.coffi - 1] &= ~ObjectFlags2.SCRDBT;
            game.Objects.oflag1[(int)ObjectIndices.trunk - 1] |= ObjectFlags.VISIBT;
            game.Rooms.RoomFlags[(int)RoomIndices.reser - 1] = (game.Rooms.RoomFlags[(int)RoomIndices.reser - 1] | RoomFlags.RLAND) & ~((int)RoomFlags.RWATER + RoomFlags.RSEEN);
            return ret_val;

        L27200:
            game.Flags.lwtidf = false;
            /* 						!YES, FILL DAM. */
            MessageHandler.Speak(212, game);
            if (ObjectHandler.qhere_((int)ObjectIndices.trunk, (int)RoomIndices.reser, game))
            {
                game.Objects.oflag1[(int)ObjectIndices.trunk - 1] &= ~ObjectFlags.VISIBT;
            }

            game.Rooms.RoomFlags[(int)RoomIndices.reser - 1] = (game.Rooms.RoomFlags[(int)RoomIndices.reser - 1] | RoomFlags.RWATER) & ~RoomFlags.RLAND;
            return ret_val;

        L27500:
            MessageHandler.rspsub_(299, odi2, game);
            /* 						!NOT WITH THAT. */
            return ret_val;

        /* O16--	GRATING */

        L28000:
            if (game.ParserVectors.prsa != (int)VIndices.openw && game.ParserVectors.prsa != (int)VIndices.closew)
            {
                goto L10;
            }
            if (game.Flags.grunlf)
            {
                goto L28200;
            }
            /* 						!UNLOCKED? */
            MessageHandler.Speak(214, game);
            /* 						!NO, LOCKED. */
            return ret_val;

        L28200:
            i = 215;
            /* 						!UNLOCKED, VIEW FRM CLR. */
            if (game.Player.Here != (int)RoomIndices.clear)
            {
                i = 216;
            }
            /* 						!VIEW FROM BELOW. */
            ret_val = RoomHandler.opncls_((int)ObjectIndices.grate, i, 885, game);
            /* 						!OPEN/CLOSE. */
            game.Rooms.RoomFlags[(int)RoomIndices.mgrat - 1] &= ~RoomFlags.RLIGHT;
            if ((game.Objects.oflag2[(int)ObjectIndices.grate - 1] & ObjectFlags2.OPENBT) != 0)
            {
                game.Rooms.RoomFlags[(int)RoomIndices.mgrat - 1] |= RoomFlags.RLIGHT;
            }
            if (!RoomHandler.IsRoomLit(game.Player.Here, game))
            {
                MessageHandler.Speak(406, game);
            }
            /* 						!IF DARK, DIE. */
            return ret_val;

        /* O17--	TRAP DOOR */

        L29000:
            if (game.Player.Here != (int)RoomIndices.lroom)
            {
                goto L29100;
            }
            /* 						!FROM LIVING ROOM? */
            ret_val = RoomHandler.opncls_((int)ObjectIndices.door, 218, 219, game);
            /* 						!OPEN/CLOSE. */
            return ret_val;

        L29100:
            if (game.Player.Here != (int)RoomIndices.cella)
            {
                goto L10;
            }
            /* 						!FROM CELLAR? */
            if (game.ParserVectors.prsa != (int)VIndices.openw || (game.Objects.oflag2[(int)ObjectIndices.door - 1] & ObjectFlags2.OPENBT) != 0)
            {
                goto L29200;
            }
            MessageHandler.Speak(220, game);
            /* 						!CANT OPEN CLOSED DOOR. */
            return ret_val;

        L29200:
            ret_val = RoomHandler.opncls_((int)ObjectIndices.door, 0, 22, game);
            /* 						!NORMAL OPEN/CLOSE. */
            return ret_val;

        /* O18--	DURABLE DOOR */

        L30000:
            i = 0;
            /* 						!ASSUME NO APPL. */
            if (game.ParserVectors.prsa == (int)VIndices.openw)
            {
                i = 221;
            }
            /* 						!OPEN? */
            if (game.ParserVectors.prsa == (int)VIndices.burnw)
            {
                i = 222;
            }
            /* 						!BURN? */
            if (game.ParserVectors.prsa == (int)VIndices.mungw)
            {
                i = game.rnd_(3) + 223;
            }
            /* 						!MUNG? */
            if (i == 0)
            {
                goto L10;
            }
            MessageHandler.Speak(i, game);
            return ret_val;

        /* O19--	MASTER SWITCH */

        L31000:
            if (game.ParserVectors.prsa != (int)VIndices.turnw)
            {
                goto L10;
            }
            /* 						!TURN? */
            if (game.ParserVectors.prsi != (int)ObjectIndices.screw)
            {
                goto L31500;
            }
            /* 						!WITH SCREWDRIVER? */
            if ((game.Objects.oflag2[(int)ObjectIndices.machi - 1] & ObjectFlags2.OPENBT) != 0)
            {
                goto L31600;
            }

            /* 						!LID UP? */
            MessageHandler.Speak(226, game);
            /* 						!NO, ACTIVATE. */
            if (game.Objects.ocan[(int)ObjectIndices.coal - 1] != (int)ObjectIndices.machi)
            {
                goto L31400;
            }

            /* 						!COAL INSIDE? */
            newsta_((int)ObjectIndices.coal, 0, 0, 0, 0, game);
            /* 						!KILL COAL, */
            newsta_((int)ObjectIndices.diamo, 0, 0, (int)ObjectIndices.machi, 0, game);
            /* 						!REPLACE WITH DIAMOND. */
            return ret_val;

        L31400:
            i__1 = game.Objects.Count;
            for (i = 1; i <= i__1; ++i)
            {
                /* 						!KILL NONCOAL OBJECTS. */
                if (game.Objects.ocan[i - 1] != (int)ObjectIndices.machi)
                {
                    goto L31450;
                }
                /* 						!INSIDE MACHINE? */
                newsta_(i, 0, 0, 0, 0, game);
                /* 						!KILL OBJECT AND CONTENTS. */
                newsta_((int)ObjectIndices.gunk, 0, 0, (int)ObjectIndices.machi, 0, game);
            /* 						!REDUCE TO GUNK. */
            L31450:
                ;
            }
            return ret_val;

        L31500:
            MessageHandler.rspsub_(300, odi2, game);
            /* 						!CANT TURN WITH THAT. */
            return ret_val;

        L31600:
            MessageHandler.Speak(227, game);
            /* 						!LID IS UP. */
            return ret_val;
        /* SOBJS, PAGE 9 */

        /* O20--	LEAK */

        L33000:
            if (game.ParserVectors.prso != (int)ObjectIndices.leak || game.ParserVectors.prsa != (int)VIndices.plugw || game.Switches.rvmnt <= 0)
            {
                goto L10;
            }

            if (game.ParserVectors.prsi != (int)ObjectIndices.putty)
            {
                goto L33100;
            }

            /* 						!WITH PUTTY? */
            game.Switches.rvmnt = -1;
            /* 						!DISABLE LEAK. */
            game.Clock.Ticks[(int)ClockIndices.cevmnt - 1] = 0;
            MessageHandler.Speak(577, game);
            return ret_val;

        L33100:
            MessageHandler.rspsub_(301, odi2, game);
            /* 						!CANT WITH THAT. */
            return ret_val;

        /* O21--	DROWNING BUTTONS */

        L34000:
            if (game.ParserVectors.prsa != (int)VIndices.pushw)
            {
                goto L10;
            }

            /* 						!PUSH? */
            switch (game.ParserVectors.prso - (int)ObjectIndices.rbutt + 1)
            {
                case 1: goto L34100;
                case 2: goto L34200;
                case 3: goto L34300;
                case 4: goto L34400;
            }

            goto L10;
        /* 						!NOT A BUTTON. */

        L34100:
            game.Rooms.RoomFlags[game.Player.Here - 1] ^= RoomFlags.RLIGHT;
            i = 230;
            if ((game.Rooms.RoomFlags[game.Player.Here - 1] & RoomFlags.RLIGHT) != 0)
            {
                i = 231;
            }
            MessageHandler.Speak(i, game);
            return ret_val;

        L34200:
            game.Flags.gatef = true;
            /* 						!RELEASE GATE. */
            MessageHandler.Speak(232, game);
            return ret_val;

        L34300:
            game.Flags.gatef = false;
            /* 						!INTERLOCK GATE. */
            MessageHandler.Speak(232, game);
            return ret_val;

        L34400:
            if (game.Switches.rvmnt != 0)
            {
                goto L34500;
            }
            /* 						!LEAK ALREADY STARTED? */
            MessageHandler.Speak(233, game);
            /* 						!NO, START LEAK. */
            game.Switches.rvmnt = 1;
            game.Clock.Ticks[(int)ClockIndices.cevmnt - 1] = -1;
            return ret_val;

        L34500:
            MessageHandler.Speak(234, game);
            /* 						!BUTTON JAMMED. */
            return ret_val;

        /* O22--	INFLATABLE BOAT */

        L36000:
            if (game.ParserVectors.prsa != (int)VIndices.inflaw)
            {
                goto L10;
            }
            /* 						!INFLATE? */
            if (game.Objects.oroom[(int)ObjectIndices.iboat - 1] != 0)
            {
                goto L36100;
            }
            /* 						!IN ROOM? */
            MessageHandler.Speak(235, game);
            /* 						!NO, JOKE. */
            return ret_val;

        L36100:
            if (game.ParserVectors.prsi != (int)ObjectIndices.pump)
            {
                goto L36200;
            }
            /* 						!WITH PUMP? */
            newsta_((int)ObjectIndices.iboat, 0, 0, 0, 0, game);
            /* 						!KILL DEFL BOAT, */
            newsta_((int)ObjectIndices.rboat, 236, game.Player.Here, 0, 0, game);
            /* 						!REPL WITH INF. */
            game.Flags.deflaf = false;
            return ret_val;

        L36200:
            i = 237;
            /* 						!JOKES. */
            if (game.ParserVectors.prsi != (int)ObjectIndices.lungs)
            {
                i = 303;
            }
            MessageHandler.rspsub_(i, odi2, game);
            return ret_val;

        /* O23--	DEFLATED BOAT */

        L37000:
            if (game.ParserVectors.prsa != (int)VIndices.inflaw)
            {
                goto L37100;
            }
            /* 						!INFLATE? */
            MessageHandler.Speak(238, game);
            /* 						!JOKE. */
            return ret_val;

        L37100:
            if (game.ParserVectors.prsa != (int)VIndices.plugw)
            {
                goto L10;
            }
            /* 						!PLUG? */
            if (game.ParserVectors.prsi != (int)ObjectIndices.putty)
            {
                goto L33100;
            }
            /* 						!WITH PUTTY? */
            newsta_((int)ObjectIndices.iboat, 239, game.Objects.oroom[(int)ObjectIndices.dboat - 1],
                game.Objects.ocan[(int)ObjectIndices.dboat - 1], game.Objects.oadv[(int)ObjectIndices.dboat - 1], game);
            newsta_((int)ObjectIndices.dboat, 0, 0, 0, 0, game);
            /* 						!KILL DEFL BOAT, REPL. */
            return ret_val;
        /* SOBJS, PAGE 10 */

        /* O24--	RUBBER BOAT */

        L38000:
            if (arg != 0)
            {
                goto L10;
            }
            /* 						!DISMISS READIN, OUT. */
            if (game.ParserVectors.prsa != (int)VIndices.boardw || game.Objects.oadv[(int)ObjectIndices.stick - 1]
                != game.Player.Winner)
            {
                goto L38100;
            }
            newsta_((int)ObjectIndices.rboat, 0, 0, 0, 0, game);
            /* 						!KILL INFL BOAT, */
            newsta_((int)ObjectIndices.dboat, 240, game.Player.Here, 0, 0, game);
            /* 						!REPL WITH DEAD. */
            game.Flags.deflaf = true;
            return ret_val;

        L38100:
            if (game.ParserVectors.prsa != (int)VIndices.inflaw)
            {
                goto L38200;
            }
            /* 						!INFLATE? */
            MessageHandler.Speak(367, game);
            /* 						!YES, JOKE. */
            return ret_val;

        L38200:
            if (game.ParserVectors.prsa != (int)VIndices.deflaw)
            {
                goto L10;
            }
            /* 						!DEFLATE? */
            if (av == (int)ObjectIndices.rboat)
            {
                goto L38300;
            }
            /* 						!IN BOAT? */
            if (game.Objects.oroom[(int)ObjectIndices.rboat - 1] == 0)
            {
                goto L38400;
            }
            /* 						!ON GROUND? */
            newsta_((int)ObjectIndices.rboat, 0, 0, 0, 0, game);
            /* 						!KILL INFL BOAT, */
            newsta_((int)ObjectIndices.iboat, 241, game.Player.Here, 0, 0, game);
            /* 						!REPL WITH DEFL. */
            game.Flags.deflaf = true;
            return ret_val;

        L38300:
            MessageHandler.Speak(242, game);
            /* 						!IN BOAT. */
            return ret_val;

        L38400:
            MessageHandler.Speak(243, game);
            /* 						!NOT ON GROUND. */
            return ret_val;

        /* O25--	BRAIDED ROPE */

        L41000:
            if (game.ParserVectors.prsa != (int)VIndices.tiew || game.ParserVectors.prso != (int)ObjectIndices.brope ||
                game.ParserVectors.prsi != (int)ObjectIndices.hook1 && game.ParserVectors.prsi !=
                (int)ObjectIndices.hook2)
            {
                goto L41500;
            }

            game.Switches.btief = game.ParserVectors.prsi;
            /* 						!RECORD LOCATION. */
            game.Clock.Flags[(int)ClockIndices.cevbal - 1] = false;
            /* 						!STALL ASCENT. */
            MessageHandler.Speak(248, game);
            return ret_val;

        L41500:
            if (game.ParserVectors.prsa != (int)VIndices.untiew || game.ParserVectors.prso != (int)ObjectIndices.brope)
            {
                goto L10;
            }

            if (game.Switches.btief != 0)
            {
                goto L41600;
            }

            /* 						!TIED UP? */
            MessageHandler.Speak(249, game);
            /* 						!NO, JOKE. */
            return ret_val;

        L41600:
            MessageHandler.Speak(250, game);
            game.Switches.btief = 0;
            /* 						!UNTIE. */

            game.Clock.Ticks[(int)ClockIndices.cevbal - 1] = 3;
            /* 						!RESTART CLOCK. */
            game.Clock.Flags[(int)ClockIndices.cevbal - 1] = true;
            return ret_val;

        /* O26--	SAFE */

        L42000:
            i = 0;
            /* 						!ASSUME UNPROCESSED. */
            if (game.ParserVectors.prsa == (int)VIndices.takew)
            {
                i = 251;
            }

            /* 						!TAKE? */
            if (game.ParserVectors.prsa == (int)VIndices.openw && game.Flags.safef)
            {
                i = 253;
            }

            /* 						!OPEN AFTER BLAST? */
            if (game.ParserVectors.prsa == (int)VIndices.openw && !game.Flags.safef)
            {
                i = 254;
            }

            /* 						!OPEN BEFORE BLAST? */
            if (game.ParserVectors.prsa == (int)VIndices.closew && game.Flags.safef)
            {
                i = 253;
            }

            /* 						!CLOSE AFTER? */
            if (game.ParserVectors.prsa == (int)VIndices.closew && !game.Flags.safef)
            {
                i = 255;
            }

            if (i == 0)
            {
                goto L10;
            }
            MessageHandler.Speak(i, game);
            return ret_val;

        /* O27--	FUSE */

        L43000:
            if (game.ParserVectors.prsa != (int)VIndices.burnw)
            {
                goto L10;
            }
            /* 						!BURN? */
            MessageHandler.Speak(256, game);
            game.Clock.Ticks[(int)ClockIndices.cevfus - 1] = 2;
            /* 						!START COUNTDOWN. */
            return ret_val;

        /* O28--	GNOME */

        L44000:
            if (game.ParserVectors.prsa != (int)VIndices.givew && game.ParserVectors.prsa != (int)VIndices.throww)
            {
                goto L44500;
            }

            if (game.Objects.otval[game.ParserVectors.prso - 1] == 0)
            {
                goto L44100;
            }

            /* 						!TREASURE? */
            MessageHandler.rspsub_(257, odo2, game);
            /* 						!YES, GET DOOR. */
            newsta_(game.ParserVectors.prso, 0, 0, 0, 0, game);
            newsta_((int)ObjectIndices.gnome, 0, 0, 0, 0, game);
            /* 						!VANISH GNOME. */
            game.Flags.gnodrf = true;
            return ret_val;

        L44100:
            MessageHandler.rspsub_(258, odo2, game);
            /* 						!NO, LOSE OBJECT. */
            newsta_(game.ParserVectors.prso, 0, 0, 0, 0, game);
            return ret_val;

        L44500:
            MessageHandler.Speak(259, game);
            /* 						!NERVOUS GNOME. */
            if (!game.Flags.gnomef)
            {
                game.Clock.Ticks[(int)ClockIndices.cevgno - 1] = 5;
            }

            /* 						!SCHEDULE BYEBYE. */
            game.Flags.gnomef = true;
            return ret_val;

        /* O29--	COKE BOTTLES */

        L46000:
            if (game.ParserVectors.prsa != (int)VIndices.throww && game.ParserVectors.prsa != (int)VIndices.mungw)
            {
                goto L10;
            }

            newsta_(game.ParserVectors.prso, 262, 0, 0, 0, game);
            /* 						!MUNG BOTTLES. */
            return ret_val;
        /* SOBJS, PAGE 11 */

        /* O30--	ROBOT */

        L53000:
            if (game.ParserVectors.prsa != (int)VIndices.givew)
            {
                goto L53200;
            }

            /* 						!GIVE? */
            newsta_(game.ParserVectors.prso, 0, 0, 0, (int)AIndices.arobot, game);
            /* 						!PUT ON ROBOT. */
            MessageHandler.rspsub_(302, odo2, game);
            return ret_val;

        L53200:
            if (game.ParserVectors.prsa != (int)VIndices.mungw && game.ParserVectors.prsa != (int)VIndices.throww)
            {
                goto L10;
            }

            newsta_((int)ObjectIndices.robot, 285, 0, 0, 0, game);
            /* 						!KILL ROBOT. */
            return ret_val;

        /* O31--	GRUE */

        L56000:
            if (game.ParserVectors.prsa != (int)VIndices.examiw)
            {
                goto L56100;
            }
            /* 						!EXAMINE? */
            MessageHandler.Speak(288, game);
            return ret_val;

        L56100:
            if (game.ParserVectors.prsa != (int)VIndices.findw)
            {
                goto L10;
            }
            /* 						!FIND? */
            MessageHandler.Speak(289, game);
            return ret_val;
        }

        /* MIRPAN--	PROCESSOR FOR GLOBAL MIRROR/PANEL */

        /* DECLARATIONS */

        public static bool mirpan_(Game game, int st, bool pnf)
        {
            /* System generated locals */
            int i__1;
            bool ret_val;

            /* Local variables */
            int num;
            int mrbf;

            ret_val = true;
            num = RoomHandler.mrhere_(game, game.Player.Here);
            /* 						!GET MIRROR NUM. */
            if (num != 0)
            {
                goto L100;
            }
            /* 						!ANY HERE? */
            MessageHandler.Speak(game, st);
            /* 						!NO, LOSE. */
            return ret_val;

        L100:
            mrbf = 0;
            /* 						!ASSUME MIRROR OK. */
            if (num == 1 && !game.Flags.mr1f || num == 2 && !game.Flags.mr2f)
            {
                mrbf = 1;
            }
            if (game.ParserVectors.prsa != (int)VIndices.movew && game.ParserVectors.prsa != (int)VIndices.openw)
            {
                goto L200;
            }
            i__1 = st + 1;
            MessageHandler.Speak(game, i__1);
            /* 						!CANT OPEN OR MOVE. */
            return ret_val;

        L200:
            if (pnf || game.ParserVectors.prsa != (int)VIndices.lookiw && game.ParserVectors.prsa !=
                (int)VIndices.examiw && game.ParserVectors.prsa != (int)VIndices.lookw)
            {
                goto L300;
            }
            i__1 = mrbf + 844;
            MessageHandler.Speak(game, i__1);
            /* 						!LOOK IN MIRROR. */
            return ret_val;

        L300:
            if (game.ParserVectors.prsa != (int)VIndices.mungw)
            {
                goto L400;
            }
            /* 						!BREAK? */
            i__1 = st + 2 + mrbf;
            MessageHandler.Speak(game, i__1);
            /* 						!DO IT. */
            if (num == 1 && !(pnf))
            {
                game.Flags.mr1f = false;
            }
            if (num == 2 && !(pnf))
            {
                game.Flags.mr2f = false;
            }
            return ret_val;

        L400:
            if (pnf || mrbf == 0)
            {
                goto L500;
            }
            /* 						!BROKEN MIRROR? */
            MessageHandler.Speak(game, 846);
            return ret_val;

        L500:
            if (game.ParserVectors.prsa != (int)VIndices.pushw)
            {
                goto L600;
            }
            /* 						!PUSH? */
            i__1 = st + 3 + num;
            MessageHandler.Speak(game, i__1);
            return ret_val;

        L600:
            ret_val = false;
            /* 						!CANT HANDLE IT. */
            return ret_val;
        }
    }
}