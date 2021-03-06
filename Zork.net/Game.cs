﻿using System;
using Zork.Core.Clock;
using Zork.Core.Helpers;
using Zork.Core.Object;
using Zork.Core.Room;

namespace Zork.Core
{
    public class Game
    {
        public Game(byte[] bytes)
        {
            this.Data = bytes;
            this.State.ltshft = 10;
            this.State.MaxScore = this.State.ltshft;
            this.State.egscor = 0;
            this.State.egmxsc = 0;
            this.State.MaxLoad = 100;
            this.State.rwscor = 0;
            this.State.Deaths = 0;
            this.State.Moves = 0;
            this.Time.pltime = 0;
            this.State.mungrm = 0;
            this.State.hs = 0;
            this.ParserVectors.prsa = 0;

            this.Hack.thfflg = false;
            this.Hack.thfact = true;
            this.Hack.swdact = false;
            this.Hack.swdsta = 0;

            this.Star.mbase = 0;

            this.Flags.buoyf = true;
            this.Flags.egyptf = true;
            this.Flags.mr1f = true;
            this.Flags.mr2f = true;
            this.Flags.follwf = true;

            this.Switches.ormtch = 4;
            this.Switches.lcell = 1;
            this.Switches.pnumb = 1;
            this.Switches.mdir = 270;
            this.Switches.mloc = (int)RoomIndices.mrb;
            this.Switches.cphere = 10;

            this.isRunning = true;
        }

        public Time Time { get; } = new Time();
        public Star Star { get; } = new Star();
        public Last Last { get; } = new Last();
        public Hack Hack { get; } = new Hack();
        public Flags Flags { get; } = new Flags();
        public Random Random { get; } = new Random(DateTime.Now.Millisecond);
        public Exits Exits { get; } = new Exits();
        public Rooms Rooms { get; } = new Rooms();
        public Switches Switches { get; } = new Switches();
        public Screen Screen { get; } = new Screen();
        public Rooms2 Rooms2 { get; } = new Rooms2();
        public Player Player { get; } = new Player();
        public Syntax Syntax { get; } = new Syntax();
        public curxt_ curxt_ { get; } = new curxt_();
        public Objects Objects { get; } = new Objects();
        public Orphans Orphans { get; } = new Orphans();
        public Villians Villians { get; } = new Villians();
        public Messages Messages { get; } = new Messages();
        public PlayerState State { get; } = new PlayerState();
        public ClockEvents Clock { get; } = new ClockEvents();
        public Adventurer Adventurers { get; } = new Adventurer();

        public hyper_ hyper_ { get; } = new hyper_();

        // TODO: Figure out naming later...
        //public ParserVector ParserVector { get; } = new ParserVector();
        public ParserVectors ParserVectors { get; } = new ParserVectors();

        public pv pv_1 { get; set; } = new pv();
        public objvec objvec { get; set; } = new objvec();
        public prpvec prpvec { get; set; } = new prpvec();

        public int DataPosition { get; set; }
        public byte[] Data { get; }

        public int astag { get; set; } = 32768;

        public static Game Initialize() => DataLoader.LoadDataFile();

        public int rnd_(int maxVal) => this.Random.Next(maxVal);

        private bool isRunning = true;

        public void Exit()
        {
            Console.WriteLine("The game is over.");
            isRunning = false;
        }

        public void Play()
        {
            MessageHandler.Speak(1, this);
            bool result = RoomHandler.RoomDescription(3, this);
            bool f = false;
            int i = 0;

            while (this.isRunning)
            {
            L100:
                if (!this.isRunning)
                {
                    // Game is over, exit this game loop.
                    continue;
                }

                this.Player.Winner = (int)AIndices.player;
                this.Player.TelFlag = false;

                string input = string.Empty;

                if (this.ParserVectors.prscon <= 1)
                {
                    input = Parser.ReadLine(1);
                    this.ParserVectors.prscon = 1;
                }

                ++this.State.Moves;
                this.ParserVectors.prswon = Parser.Parse(input, true, this);

                if (!this.ParserVectors.prswon)
                {
                    goto L400;
                }

                if (xvehic_(1))
                {
                    goto L400;
                }

                if (this.ParserVectors.prsa == (int)VIndices.tellw)
                {
                    //goto L2000;
                }

            L300:
                if (this.ParserVectors.prso == (int)ObjectIndices.valua || this.ParserVectors.prso == (int)ObjectIndices.every)
                {
                    goto L900;
                }

                if (!Parser.vappli_(input, this.ParserVectors.prsa, this))
                {
                    goto L400;
                }

            L350:
                if (!Flags.echof && this.Player.Here == (int)RoomIndices.echor)
                {
                    goto L1000;
                }

                f = RoomHandler.rappli_(this.Rooms.RoomActions[this.Player.Here - 1], this);

            L400:
                xendmv_(this.Player.TelFlag);

                // !DO END OF MOVE.
                if (!RoomHandler.IsRoomLit(this.Player.Here, this))
                {
                    this.ParserVectors.prscon = 1;
                }

                goto L100;

            L900:
                dverb1.valuac_(this, (int)ObjectIndices.valua);
                goto L350;
            // GAME, PAGE 3

            // SPECIAL CASE-- ECHO ROOM.
            // IF INPUT IS NOT 'ECHO' OR A DIRECTION, JUST ECHO.

            L1000:
                input = Parser.ReadLine(0);

                // !CHARGE FOR MOVES.
                ++this.State.Moves;

                if (input.Equals("ECHO"))
                    goto L1300;

                MessageHandler.Speak(571, this);

                // !KILL THE ECHO.
                Flags.echof = true;
                this.Objects.oflag2[(int)ObjectIndices.bar - 1] &= ~ObjectFlags2.SCRDBT;
                this.ParserVectors.prswon = true;
                // !FAKE OUT PARSER.
                this.ParserVectors.prscon = 1;
                // !FORCE NEW INPUT.
                goto L400;

            L1300:
                this.ParserVectors.prswon = Parser.Parse(input, false, this);
                if (!this.ParserVectors.prswon || this.ParserVectors.prsa != (int)VIndices.walkw)
                {
                    goto L1400;
                }
                if (dso3.findxt_(this, this.ParserVectors.prso, this.Player.Here))
                {
                    goto L300;
                }
            // !VALID EXIT?

            L1400:
                MessageHandler.more_output(input);
                this.Player.TelFlag = true;
                // !INDICATE OUTPUT.
                goto L1000;
            // !MORE ECHO ROOM.
            // GAME, PAGE 4

            // SPECIAL CASE-- TELL <ACTOR>, NEW COMMAND
            // NOTE THAT WE CANNOT BE IN THE ECHO ROOM.

            L2000:
                if ((this.Objects.oflag2[this.ParserVectors.prso - 1] & ObjectFlags2.ACTRBT) != 0)
                {
                    goto L2100;
                }

                MessageHandler.Speak(602, this);
                // !CANT DO IT.
                goto L350;
            // !VAPPLI SUCCEEDS.

            L2100:
                this.Player.Winner = ObjectHandler.GetActor(this.ParserVectors.prso, this);
                // !NEW PLAYER.
                this.Player.Here = this.Adventurers.Rooms[this.Player.Winner - 1];

                // !NEW LOCATION.
                if (this.ParserVectors.prscon <= 1)
                {
                    goto L2700;
                }

                // !ANY INPUT?
                if (Parser.Parse(input, true, this))
                {
                    goto L2150;
                }

            L2700:
                i = 341;
                // !FAILS.
                if (this.Player.TelFlag)
                {
                    i = 604;
                }
                // !GIVE RESPONSE.
                MessageHandler.Speak(i, this);

            L2600:
                this.Player.Winner = (int)AIndices.player;
                // !RESTORE STATE.
                this.Player.Here = this.Adventurers.Rooms[this.Player.Winner - 1];
                goto L350;

            L2150:
                //if (ObjectHandler.aappli_(this.Adventurers.Actions[this.Player.Winner - 1], game))
                //{
                //    goto L2400;
                //}

                // !ACTOR HANDLE?
                //if (xvehic_(1))
                //{
                //    goto L2400;
                //}

                // !VEHICLE HANDLE?
                if (this.ParserVectors.prso == (int)ObjectIndices.valua || this.ParserVectors.prso == (int)ObjectIndices.every)
                {
                    goto L2900;
                }
                if (!Parser.vappli_(input, this.ParserVectors.prsa, this))
                {
                    goto L2400;
                }
                // !VERB HANDLE?
                // L2350:
                f = RoomHandler.rappli_(this.Rooms.RoomActions[this.Player.Here - 1], this);

            L2400:
                xendmv_(this.Player.TelFlag);
                // !DO END OF MOVE.
                goto L2600;
            // !DONE.

            L2900:
                dverb1.valuac_(this, (int)ObjectIndices.valua);
                // !ALL OR VALUABLES.
                goto L350;
            }
        }

        /* XENDMV-	EXECUTE END OF MOVE FUNCTIONS. */

        public void xendmv_(bool flag)
        {
            bool f;

            if (!(flag))
            {
                MessageHandler.rspeak_(this, 341);
            }
            /* 						!DEFAULT REMARK. */
            if (this.Hack.thfact)
            {
                actors.thiefd_(this);
            }
            /* 						!THIEF DEMON. */
            if (this.ParserVectors.prswon)
            {
                DemonHandler.fightd_(this);
            }
            /* 						!FIGHT DEMON. */
            if (this.Hack.swdact)
            {
                DemonHandler.swordd_(this);
            }
            /* 						!SWORD DEMON. */
            if (this.ParserVectors.prswon)
            {
                f = ClockEvents.clockd_(this);
            }
            /* 						!CLOCK DEMON. */
            if (this.ParserVectors.prswon)
            {
                f = xvehic_(2);
            }
            /* 						!VEHICLE READOUT. */
        }

        /* XVEHIC- EXECUTE VEHICLE FUNCTION */

        public bool xvehic_(int n)
        {
            bool ret_val;
            int av;

            ret_val = false;
            /* 						!ASSUME LOSES. */
            av = this.Adventurers.Vehicles[this.Player.Winner - 1];
            /* 						!GET VEHICLE. */
            if (av != 0)
            {
                ret_val = ObjectHandler.oappli_(this.Objects.oactio[av - 1], n, this);
            }

            return ret_val;
        }
    }

    public class hyper_
    {
        public int hfactr { get; set; } = 500;
    }

    public class curxt_
    {
        public int xtype { get; set; }
        public int xroom1 { get; set; }
        public int xstrng { get; set; }
        public int xactio { get; set; }
        public int xobj { get; set; }
    }
}