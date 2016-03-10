﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Spider_web.Utils;

namespace Spider_web.Models
{
    internal class UserLvl
    {
        public int LvlNumber, SpidersQuantity, ContactsQuantity, Difficult;
        public bool IsFinished;

        public List<SpidersCoordinates> Coordinatese;
        public List<SpidersContacts> Contacts;

        public override string ToString()
        {
            return $"{LvlNumber} {Difficult}";
        }
    }

    internal class DefaultLvl
    {
        public string CoordinatesString, ContactsString;

        public List<SpidersCoordinates> Coordinates;
        public List<SpidersContacts> Contacts;
    }

    internal struct SpidersCoordinates
    {
        public double X, Y;

        public SpidersCoordinates(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    internal struct SpidersContacts
    {
        public int ContactStart, ContactStop;

        public SpidersContacts(int start, int stop)
        {
            ContactStart = start;
            ContactStop = stop;
        }
    }

    internal class LvlFactory
    {
        private static List<DefaultLvl> _defaultLvls;
        public List<UserLvl> UserLvls;

        //Инициализирует уровни
        public LvlFactory()
        {
            _defaultLvls = new List<DefaultLvl>
            {
                new DefaultLvl
                {
                    CoordinatesString = "50 10 15 50 85 50 25 90 75 90",//25
                    ContactsString = "0 1 0 2 0 3 0 4 1 2"
                },
                new DefaultLvl
                {
                    CoordinatesString = "25 10 75 10 10 50 90 50 25 90 75 90",//66
                    ContactsString = "0 1 0 2 0 3 1 2 1 3 2 3 2 4 2 5 3 4 3 5 4 5"
                },
                new DefaultLvl
                {
                    CoordinatesString = "50 10 20 20 80 20 10 50 90 50 20 80 80 80 50 90",//96
                    ContactsString = "0 1 0 2 0 3 0 4 1 2 1 5 2 6 3 7 4 7 5 6 5 7 6 7"
                },
                new DefaultLvl
                {
                    CoordinatesString = "10 10 10 50 10 90 50 50 90 10 90 50 90 90",//63
                    ContactsString = "0 1 0 6 1 2 1 5 2 3 2 4 3 4 4 5 5 6"
                },
                new DefaultLvl
                {
                    CoordinatesString = "50 10 20 20 80 20 10 50 90 50 20 80 80 80 50 90",//80
                    ContactsString = "0 2 0 5 1 3 1 6 2 4 3 4 3 7 4 5 4 7 6 7"
                },
                new DefaultLvl
                {
                    CoordinatesString = "11 6 90 94 31 51 10 94 90 6",//40
                    ContactsString = "0 2 0 1 1 2 2 3 3 4 4 2 4 0 3 1"
                },
                new DefaultLvl
                {
                    CoordinatesString = "49 9 12 95 12 45 88 93 88 45",//40
                    ContactsString = "1 2 2 3 3 0 0 2 2 4 4 3 0 1 1 4"
                },
                new DefaultLvl
                {
                    CoordinatesString = "50 10 20 20 80 20 10 50 90 50 20 80 80 80 50 90",//96
                    ContactsString = "0 1 0 3 0 6 1 2 1 4 2 4 2 7 3 5 3 6 4 6 5 6 6 7"
                },
                new DefaultLvl
                {
                    CoordinatesString = "30 10 70 90 30 30 30 70 70 50 70 70 70 30 30 50 70 10 30 90",//130
                    ContactsString = "0 1 0 3 1 2 1 3 2 4 3 4 5 6 5 7 5 8 6 7 7 8 7 9 8 9"
                },
                new DefaultLvl
                {
                    CoordinatesString = "50 10 10 35 40 35 60 35 90 35 30 60 70 60 50 70 18 90 82 90",//130
                    ContactsString = "0 7 0 8 0 9 1 4 1 7 2 3 2 5 3 6 4 7 5 7 6 7 7 8 7 9"
                },
                new DefaultLvl
                {
                    CoordinatesString = "30 70 50 90 70 70 50 50 50 10 90 90 10 90 90 50 10 50",//144
                    ContactsString = "0 1 0 3 0 6 1 2 1 4 2 4 2 7 3 5 3 6 4 6 5 6 6 7 7 8 0 8 1 8 5 8"
                },
                new DefaultLvl
                {
                    CoordinatesString = "30 30 50 50 10 10 30 70 90 90 70 70 70 30 90 10 10 90",//144
                    ContactsString = "0 5 0 7 0 4 5 3 5 1 7 2 7 6 7 3 3 6 3 8 6 2 6 8 2 4 2 8 8 1 4 1"
                },
                new DefaultLvl
                {
                    CoordinatesString = "93 96 78 74 7 96 78 26 51 4 24 26 24 74 8 4 51 50 93 4",//150
                    ContactsString = "9 1 1 8 8 7 7 3 3 6 6 9 3 4 4 0 0 2 2 5 5 3 4 5 6 7 7 5 6 4"
                },
                new DefaultLvl
                {
                    CoordinatesString = "30 10 70 10 50 30 10 50 90 50 20 70 80 70 30 90 50 90 70 90",//210
                    ContactsString = "5 7 5 2 5 6 6 8 2 7 2 3 2 0 2 9 2 1 2 4 2 8 7 3 3 0 0 9 9 1 1 4 4 8 5 3 3 9 9 4 4 6"
                },
                new DefaultLvl
                {
                    CoordinatesString = "38 10 83 75 17 75 83 25 38 90 62 90 10 50 62 10 17 25 90 50",//180
                    ContactsString = "0 2 0 3 0 9 1 2 1 4 1 5 2 3 2 4 3 6 4 5 4 6 5 6 5 7 6 7 6 9 7 8 7 9 8 9"
                },
                new DefaultLvl
                {
                    CoordinatesString = "71 78 64 19 15 41 50 56 60 67 63 28 17 49 76 50 36 21 50 74 81 42 36 29 40 68 31 77",//182
                    ContactsString = "4 3 6 5 8 7 10 9 0 1 1 12 0 2 2 12 1 2 11 12 12 13 1 11 2 13"
                },
                new DefaultLvl
                {
                    CoordinatesString = "71 41 52 67 37 5 74 80 8 21 68 5 35 41 34 80 92 94 93 18 7 96",//198
                    ContactsString = "0 1 1 2 2 3 3 4 4 5 5 6 6 7 7 8 8 3 3 9 9 10 9 2 1 9 0 10 10 1 9 4 2 8 8 5"
                },
                new DefaultLvl
                {
                    CoordinatesString = "38 10 83 75 17 75 83 25 38 90 62 90 10 50 62 10 17 25 90 50 50 50",//187
                    ContactsString = "0 1 0 3 1 2 2 3 2 4 2 5 3 4 3 6 4 5 4 6 4 7 5 7 5 10 6 7 6 8 8 9 9 10"
                },
                new DefaultLvl
                {
                    CoordinatesString = "38 10 83 75 17 75 83 25 38 90 62 90 10 50 62 10 17 25 90 50 30 50 70 50",//252
                    ContactsString = "0 1 0 6 1 2 1 6 1 7 1 8 2 3 2 8 3 4 3 8 3 9 3 10 4 5 4 10 5 10 5 11 6 7 7 8 8 9 9 10 10 11"
                },
                new DefaultLvl
                {
                    CoordinatesString = "81 64 23 65 10 91 92 6 10 9 10 33 90 28 50 48 50 17 51 76 91 89",//209
                    ContactsString = "3 10 10 4 4 9 9 3 3 8 8 4 7 8 8 0 1 8 8 6 6 7 1 0 3 2 2 8 8 5 5 6 2 1 0 2 5 7"
                },
                new DefaultLvl
                {
                    CoordinatesString = "79 74 25 74 69 12 88 22 53 63 36 36 92 95 54 4 9 93 53 50 12 23 36 12 70 36",//247
                    ContactsString = "11 9 9 8 8 10 10 12 12 11 9 10 8 7 7 6 6 4 4 2 2 0 0 1 1 3 3 5 5 7 5 1 2 6 5 6 2 1"
                },
                new DefaultLvl
                {
                    CoordinatesString = "38 10 83 75 17 75 83 25 38 90 62 90 10 50 62 10 17 25 90 50 30 50 70 50",//276
                    ContactsString = "0 1 0 2 0 3 0 4 0 8 1 3 2 4 2 5 2 8 3 4 3 5 4 5 5 6 5 7 6 7 6 10 6 11 7 8 7 10 8 9 8 10 8 11 9 11"
                },
                new DefaultLvl
                {
                    CoordinatesString = "50 10 50 20 50 30 45 50 55 50 35 70 45 70 55 70 65 70 50 65 35 90 30 90 70 90 65 90",//294
                    ContactsString = "0 1 1 2 2 3 2 4 2 6 2 7 3 4 3 5 4 8 5 10 5 11 6 9 6 11 7 9 7 12 8 12 8 13 9 11 9 12 10 11 12 13"
                },
                new DefaultLvl
                {
                    CoordinatesString = "10 38 38 90 62 38 90 38 62 62 62 10 10 62 38 10 38 38 62 90 90 62 38 62",//324
                    ContactsString = "0 1 0 3 0 9 1 2 1 3 1 4 1 5 1 6 2 6 2 11 3 4 3 7 3 9 4 5 4 7 5 6 5 7 5 8 6 8 6 11 7 8 7 9 7 10 8 10 8 11 9 10 10 11"
                },
                new DefaultLvl
                {
                    CoordinatesString = "38 10 62 10 10 38 38 38 62 38 90 38 10 62 38 62 62 62 90 62 38 90 62 90 50 50",//273
                    ContactsString = "0 3 0 4 0 5 0 6 0 9 0 10 0 11 0 12 1 2 1 8 2 3 2 7 3 4 3 6 4 5 6 9 7 8 8 10 9 10 10 12 11 12"
                },
                new DefaultLvl
                {
                    CoordinatesString = "38 10 62 10 10 38 38 38 62 38 90 38 10 62 38 62 62 62 90 62 38 90 62 90 15 15 85 85",//308
                    ContactsString = "0 2 0 3 1 2 1 5 1 6 2 3 2 6 3 4 3 7 4 7 4 8 5 6 5 9 6 9 7 8 7 12 8 12 9 13 10 11 10 13 11 13 12 13"
                },
                new DefaultLvl
                {
                    CoordinatesString = "67 79 52 64 29 96 36 47 64 28 86 47 43 27 53 37 21 47 76 96 53 4 43 14 72 47 66 15 53 50 38 78",//320
                    ContactsString = "4 3 3 2 2 5 5 6 2 1 1 7 7 8 1 9 9 10 1 0 0 15 15 14 14 13 13 12 12 14 12 11 11 0 11 15 2 9 2 7"
                },
                new DefaultLvl
                {
                    CoordinatesString = "70 48 7 96 66 18 77 86 56 75 38 68 56 30 46 20 56 55 93 96 70 68 27 86 42 48 56 4",//322
                    ContactsString = "0 7 7 8 8 10 10 13 13 1 1 12 12 11 11 9 9 6 6 2 6 5 5 7 8 5 5 9 9 3 3 10 3 8 3 11 11 4 4 10 4 12 4 13 7 6"
                },
                new DefaultLvl
                {
                    CoordinatesString = "38 10 62 10 10 38 38 38 62 38 90 38 10 62 38 62 62 62 90 62 38 90 62 90 15 15 85 85 50 50",//420
                    ContactsString = "0 1 0 2 1 2 1 6 1 7 2 3 2 7 3 8 4 5 4 9 4 10 4 11 4 14 5 11 6 7 6 14 7 8 7 12 7 13 8 13 9 10 9 11 9 14 10 11 11 14 12 13 12 14 13 14"
                },
                new DefaultLvl
                {
                    CoordinatesString = "10 10 90 90 30 10 70 90 70 10 30 90 90 10 10 90 10 70 10 30 30 70 30 30 90 70 90 30 70 70 70 30",//368
                    ContactsString = "0 1 0 2 1 3 2 3 4 5 4 6 7 6 7 5 8 9 8 10 11 9 11 10 12 13 14 12 15 14 15 13 2 12 1 8 11 5 15 6 13 10 7 4 0 3"
                },
                new DefaultLvl
                {
                    CoordinatesString = "10 66 85 60 30 75 46 65 22 27 76 66 31 61 46 36 85 75 45 27 62 66 10 46 93 66 9 56 46 55 15 35 46 45",//425
                    ContactsString = "15 1 1 16 16 4 4 15 4 1 4 6 6 3 3 5 5 2 2 0 0 3 3 2 2 7 7 8 8 9 9 10 10 11 11 12 12 13 13 14 14 3 14 6 14 4 13 8 9 12"
                },
                new DefaultLvl
                {
                    CoordinatesString = "10 10 90 90 30 10 70 90 70 10 30 90 90 10 10 90 10 70 10 30 30 70 30 30 90 70 90 30 70 70 70 30 50 50",//442
                    ContactsString = "0 1 0 2 1 3 2 3 4 5 4 6 7 6 7 5 8 9 8 10 11 9 11 10 12 13 14 12 15 14 15 13 2 12 1 8 11 5 15 6 7 4 0 3 16 3 16 10 16 4 16 13"
                },
                new DefaultLvl
                {
                    CoordinatesString = "38 10 62 10 10 38 38 38 62 38 90 38 10 62 38 62 62 62 90 62 38 90 62 90 15 15 85 85 85 15 15 85",//448
                    ContactsString = "0 1 0 8 0 12 1 4 1 12 2 4 2 5 3 5 3 6 3 7 4 12 4 13 4 5 5 13 5 14 6 7 6 14 6 15 7 15 8 9 8 12 9 12 9 13 10 13 10 14 11 14 11 15 14 15"
                },
                new DefaultLvl
                {
                    CoordinatesString = "38 10 62 10 10 38 38 38 62 38 90 38 10 62 38 62 62 62 90 62 38 90 62 90 15 15 85 85 85 15 15 85 50 50",//442
                    ContactsString = "0 3 0 4 0 8 1 2 1 7 2 3 2 7 3 8 4 5 4 8 5 6 5 9 6 9 7 8 7 10 8 9 8 10 8 11 8 12 9 11 10 13 11 14 12 13 12 14 13 15 14 16"
                },
                new DefaultLvl
                {
                    CoordinatesString = "23 5 52 37 28 19 9 47 9 14 44 5 31 54 92 93 80 61 9 28 85 6 90 75 64 4 65 50 29 35 72 20 9 65 50 17",//450
                    ContactsString = "9 10 10 11 11 12 12 13 13 8 8 4 4 1 1 0 0 2 2 2 2 3 3 7 7 6 6 5 5 9 11 14 14 15 15 16 16 17 17 15 15 11 14 10 10 7 7 4 12 8"
                },
                new DefaultLvl
                {
                    CoordinatesString = "45 36 31 46 12 54 62 37 65 14 48 53 30 96 53 69 81 14 93 36 55 25 47 86 87 26 9 70 72 26 29 71 13 87 79 37 75 5",//513
                    ContactsString = "9 10 10 11 11 12 12 13 13 14 14 15 15 16 16 18 18 17 17 10 9 17 1 4 4 0 0 7 7 2 2 6 6 3 3 5 5 1 4 5 7 6 6 5 4 7 8 4 8 5 8 6 8 7"
                },
                new DefaultLvl
                {
                    CoordinatesString = "8 64 93 4 48 63 46 93 8 93 62 22 93 21 62 4 78 38 46 77 28 63 78 22 62 38 93 37 9 78 79 4 28 93 28 77",//522
                    ContactsString = "0 1 1 2 2 3 3 4 4 8 8 5 5 1 0 6 6 7 6 5 5 3 8 7 7 17 8 16 4 15 6 12 0 11 11 10 10 9 9 14 14 15 15 16 16 13 13 10 11 12 12 17 17 16 13 12 13 14"
                },
                new DefaultLvl
                {
                    CoordinatesString = "85 6 18 19 52 93 52 35 84 64 53 5 85 20 52 63 19 63 85 50 84 80 84 36 52 49 18 5 83 95 18 35 52 78 19 94 19 79 20 49",//680
                    ContactsString = "0 1 1 2 0 3 3 4 4 5 5 6 6 7 7 2 4 8 4 16 16 6 6 17 17 18 18 19 8 18 18 9 9 15 15 10 10 12 12 11 12 13 13 14 14 12 14 15 15 19 8 9 9 10 10 11 14 19 19 17 5 16 16 18 18 15 15 12"
                },
                new DefaultLvl
                {
                    CoordinatesString = "38 10 62 10 10 38 38 38 62 38 90 38 10 62 38 62 62 62 90 62 10 90 50 90 90 90 25 25 75 75 75 25 25 75 50 50",//540
                    ContactsString = "0 1 0 6 0 7 1 2 1 7 2 3 2 7 3 4 3 8 4 5 4 8 5 8 5 9 6 7 6 10 7 10 8 9 8 14 9 14 10 15 11 12 11 13 11 16 12 15 12 16 13 16 13 17 14 17 15 16 16 17"
                },
                new DefaultLvl
                {
                    CoordinatesString = "73 60 90 63 16 74 73 14 55 6 59 29 8 42 89 42 11 29 84 27 35 8 28 84 76 86 72 45 87 76 10 59 44 90 56 51 60 91 20 15",//640
                    ContactsString = "7 6 6 19 7 19 19 5 5 4 4 19 19 3 3 2 2 19 19 1 1 0 0 19 0 18 18 16 16 15 15 18 18 14 14 15 18 17 17 14 17 13 13 0 0 17 13 11 11 10 10 13 10 9 9 0 0 10 9 12 12 19 8 7"
                },
                new DefaultLvl
                {
                    CoordinatesString = "10 10 50 10 90 10 10 38 38 38 62 38 90 38 10 62 38 62 62 62 90 62 10 90 50 90 90 90 15 15 75 75 75 25 15 85 50 50",//684
                    ContactsString = "0 2 0 3 0 7 2 7 2 8 3 7 3 9 3 10 7 8 7 9 8 9 9 10 1 4 1 5 1 6 4 5 4 10 4 11 5 6 5 11 5 12 6 12 10 11 11 12 10 13 10 14 13 14 13 15 13 16 14 15 14 18 15 16 15 17 15 18 16 17 17 18"
                },
                new DefaultLvl
                {
                    CoordinatesString = "10 10 90 90 30 10 70 90 50 10 50 90 70 10 30 90 90 10 10 90 10 70 90 30 30 70 70 30 50 70 50 30 30 30 70 70 10 30 90 70",//760
                    ContactsString = "0 2 0 3 0 7 2 7 2 8 3 7 3 9 3 10 7 8 7 9 8 9 9 10 1 4 1 5 1 6 4 5 4 10 4 11 5 6 5 11 5 12 6 12 10 11 11 12 10 13 10 14 13 14 13 15 13 16 14 15 14 18 15 16 15 17 15 18 16 17 17 18 9 19 3 19"
                },
                new DefaultLvl
                {
                    CoordinatesString = "10 10 50 10 90 10 10 50 50 50 10 90 50 90 90 90 20 50 50 20 50 80 80 50 40 40 60 40 40 60 60 60 27 27 77 27 27 77 77 77",//760
                    ContactsString = "0 2 0 3 0 7 2 7 2 8 3 7 3 9 3 10 7 8 7 9 8 9 9 10 1 4 1 5 1 6 4 5 4 10 4 11 5 6 5 11 5 12 6 12 10 11 11 12 10 13 10 14 13 14 13 15 13 16 14 15 14 18 15 16 15 17 15 18 16 17 17 18 9 19 3 19"
                },
                new DefaultLvl
                {
                    CoordinatesString = "10 10 90 90 30 10 70 90 50 10 50 90 70 10 30 90 90 10 10 90 10 70 90 30 30 70 70 30 50 70 50 30 30 30 70 70 10 30 90 70 50 50",//735
                    ContactsString = "0 1 0 2 0 3 0 4 1 3 2 4 3 4 5 6 5 7 5 8 5 9 6 8 7 9 8 9 10 11 10 12 10 13 10 14 11 13 12 14 13 14 15 16 15 17 15 18 15 19 16 18 17 19 18 19 20 0 20 5 20 10 20 15 0 5 5 10 10 15"
                },
                new DefaultLvl
                {
                    CoordinatesString = "10 10 90 90 30 10 70 90 50 10 50 90 70 10 30 90 90 10 10 90 10 70 90 30 30 70 70 30 50 70 50 30 30 30 70 70 10 30 90 70 10 50 90 50 30 50 70 50 50 50",//1000
                    ContactsString = "0 1 1 2 2 3 3 4 4 9 9 8 8 3 8 7 7 2 7 6 6 1 6 5 5 0 5 21 21 22 22 6 22 20 20 7 20 24 24 8 24 23 23 9 23 14 14 13 13 24 13 12 12 20 12 11 11 22 11 10 10 21 10 15 15 16 16 11 16 17 17 12 17 18 18 13 18 19 19 14"
                },
                new DefaultLvl
                {
                    CoordinatesString = "40 10 65 10 35 20 45 20 60 20 70 20 15 35 30 35 75 35 90 35 15 45 35 45 70 45 90 45 15 55 35 55 70 55 90 55 35 65 70 65 45 75 60 75 45 85 60 85 45 95 60 95",//1118
                    ContactsString = "0 1 1 2 2 3 3 4 4 9 9 8 8 3 8 7 7 2 7 6 6 1 6 5 5 0 5 21 21 22 22 6 22 20 20 7 20 24 24 8 24 23 23 9 23 14 14 13 13 24 13 12 12 20 12 11 11 22 11 10 10 21 10 15 15 16 16 11 16 17 17 12 17 18 18 13 18 19 19 14 0 25 1 25 5 25"
                },
                new DefaultLvl
                {
                    CoordinatesString = "42 26 9 51 61 80 75 59 88 77 19 81 95 58 46 78 29 43 55 92 28 61 11 26 54 51 61 9 8 39 8 68 38 8 74 41 20 14 92 39 35 89 56 27 87 23 75 15 75 91",//975
                    ContactsString = "15 0 0 1 1 2 2 3 3 4 4 5 5 6 6 7 7 8 8 9 9 10 10 11 11 12 12 13 13 14 14 15 15 17 14 17 17 24 24 20 20 6 24 21 21 7 24 16 16 13 16 12 16 22 22 11 22 24 23 21 23 9 23 8 20 19 19 4 18 1 18 0 18 2 18 19 18 24"
                },
                new DefaultLvl
                {
                    CoordinatesString = "8 41 74 9 27 10 51 48 74 90 50 86 10 29 51 14 91 67 7 55 50 32 29 90 93 54 65 67 42 4 85 17 42 96 59 95 23 48 15 17 80 48 58 4 8 68 36 67 86 79 14 80 91 39 89 27",//1008
                    ContactsString = "0 3 3 5 5 6 6 7 7 4 4 2 2 1 1 0 3 4 9 10 10 8 8 12 12 15 15 13 13 14 14 11 11 13 9 11 11 10 17 18 18 19 19 20 20 21 21 16 16 16 16 17 16 19 24 25 25 26 26 27 27 22 22 23 23 24 24 22 22 26 26 24"
                },
                new DefaultLvl
                {
                    CoordinatesString = "32 85 91 18 37 46 90 29 72 12 47 20 15 70 86 9 46 62 53 96 86 40 87 52 22 32 62 20 9 59 66 43 54 70 84 70 38 14 19 21 43 91 75 81 13 40 22 11 65 90 61 62 88 60 23 78 9 49",//1450
                    ContactsString = "11 10 10 9 9 8 8 6 6 5 5 4 4 3 3 2 2 1 1 0 0 28 28 27 27 26 26 22 22 21 21 20 20 19 19 18 18 17 17 16 16 15 15 14 14 12 12 11 10 12 12 9 9 14 9 7 7 8 7 14 14 13 13 15 13 16 16 17 16 18 16 19 19 13 21 13 26 25 25 24 24 23 23 25 25 13 23 7 6 23 3 23 2 23 0 24 28 24 27 25"
                },
                new DefaultLvl
                {
                    CoordinatesString = "15 37 64 15 75 26 25 58 58 42 57 58 40 41 41 58 7 47 41 30 75 45 49 7 28 26 26 83 75 57 93 46 26 91 49 21 26 71 25 46 56 29 57 95 75 95 83 36 57 71 75 70 41 70 40 95 75 83 38 17",//1500
                    ContactsString = "27 28 28 29 29 27 28 4 4 2 2 3 3 4 4 9 9 7 7 8 8 9 7 6 6 20 20 19 19 1 1 5 5 6 6 19 20 1 9 27 7 18 18 17 17 16 16 15 15 18 18 16 16 14 14 0 0 13 13 14 17 12 12 21 21 10 10 11 11 12 21 8 17 26 26 22 22 24 24 23 23 25 25 24 22 23 25 26 26 23 22 13 0 1 20 15 25 11 10 27"
                },
                new DefaultLvl
                {
                    CoordinatesString = "51 64 76 53 64 49 51 55 86 80 12 80 51 21 49 46 74 13 68 93 35 91 33 44 50 5 86 61 19 39 52 74 88 70 89 19 12 89 79 87 51 36 22 12 52 84 88 11 11 30 51 29 34 7 14 21 51 94 64 7 23 86 51 13",//1856
                    ContactsString = "13 15 15 27 27 12 12 14 14 13 13 27 12 3 3 1 1 0 0 23 23 1 23 24 24 22 22 2 2 23 2 0 2 11 22 11 11 10 10 6 6 5 5 25 25 7 7 4 4 5 25 10 7 8 8 26 26 25 26 10 26 9 9 8 9 18 18 19 19 20 20 21 21 17 17 30 30 16 16 28 28 31 31 20 31 17 31 30 31 29 29 19 29 18 29 28 31 21 29 13 28 13 14 16 14 28 14 3 27 26 27 10 27 22 27 9"
                },
                new DefaultLvl
                {
                    CoordinatesString = "91 86 75 96 91 41 9 43 84 12 92 51 33 17 8 86 50 18 74 4 91 77 27 4 12 32 43 4 58 4 58 51 49 69 36 69 49 58 8 4 49 88 63 69 92 96 25 96 8 76 49 80 75 86 70 18 39 96 60 96 49 33 49 41 93 4 41 11 8 53 69 77 24 86 19 13 40 51 29 77 89 31 63 12 8 96",//3096
                    ContactsString = "9 7 9 6 9 5 9 4 9 3 9 2 9 1 9 0 9 8 0 8 8 7 6 5 4 5 3 2 2 1 17 20 20 19 20 11 20 13 20 15 17 16 16 15 15 14 14 13 13 12 12 11 11 10 10 19 19 18 18 17 30 31 31 32 32 33 33 34 34 35 35 36 36 37 37 38 38 39 39 40 40 41 41 30 30 42 42 40 42 38 42 36 42 34 42 32 26 28 26 29 26 21 26 22 26 23 26 24 26 25 26 27 27 25 25 24 24 23 23 22 22 21 21 29 29 28 28 27 29 37 28 35 27 33 25 31 6 17 5 15 7 19 3 24"
                }
            };

            InitDefaultLvls();
            UserLvls = new List<UserLvl>();
            InitUserLvlS();
        }

        //Парсит уровни
        public void InitDefaultLvls()
        {
            foreach (var defaultLvl in _defaultLvls)
            {
                defaultLvl.Coordinates = new List<SpidersCoordinates>();
                var coordinatesArray = defaultLvl.CoordinatesString.Split(' ');
                for (var j = 0; j < coordinatesArray.Length; j += 2)
                {
                    defaultLvl.Coordinates.Add(new SpidersCoordinates
                    {
                        X = Convert.ToInt32(coordinatesArray[j]),
                        Y = Convert.ToInt32(coordinatesArray[j + 1])
                    });
                }

                defaultLvl.Contacts = new List<SpidersContacts>();
                var contactsArray = defaultLvl.ContactsString.Split(' ');
                for (var j = 0; j < contactsArray.Length; j += 2)
                {
                    defaultLvl.Contacts.Add(new SpidersContacts
                    {
                        ContactStart = Convert.ToInt32(contactsArray[j]),
                        ContactStop = Convert.ToInt32(contactsArray[j + 1])
                    });
                }
            }
        }

        //Инициализирует пользовательские уровни
        public async void InitUserLvlS()
        {
            for (var i = 0; i < _defaultLvls.Count; i++)
            {
                UserLvls.Add(new UserLvl());
                var userLvl = UserLvls[i];
                var defaultLvl = _defaultLvls[i];

                userLvl.LvlNumber = i + 1;
                userLvl.SpidersQuantity = defaultLvl.Coordinates.Count;
                userLvl.ContactsQuantity = defaultLvl.Contacts.Count;

                userLvl.IsFinished = false;
                userLvl.Difficult = userLvl.SpidersQuantity * userLvl.ContactsQuantity;

                userLvl.Coordinatese = new List<SpidersCoordinates>();
                foreach (var coordinates in defaultLvl.Coordinates)
                {
                    userLvl.Coordinatese.Add(new SpidersCoordinates
                    {
                        X = double.Parse(coordinates.X.ToString()),
                        Y = double.Parse(coordinates.Y.ToString())
                    });
                }

                userLvl.Contacts = new List<SpidersContacts>();
                foreach (var contacts in defaultLvl.Contacts)
                    userLvl.Contacts.Add(contacts);
            }

            var passedLvls = await Settings.GetPassedLvls();
            foreach (var lvl in passedLvls.Where(lvl => lvl >= 0))
            {
                UserLvls[lvl].IsFinished = true;
            }
        }

        //Перезагружает координаты в пользовательских уровнях
        public static List<UserLvl> RefreshUserLvlS(List<UserLvl> userLvls)
        {
            for (var i = 0; i < _defaultLvls.Count; i++)
            {
                var userLvl = userLvls[i];
                var defaultLvl = _defaultLvls[i];
                
                for (var j = 0; j < defaultLvl.Coordinates.Count; j++)
                {
                    userLvl.Coordinatese[j] = new SpidersCoordinates(double.Parse(defaultLvl.Coordinates[j].X.ToString()),
                        double.Parse(defaultLvl.Coordinates[j].Y.ToString()));
                }
            }
            return userLvls;
        }
    }
}