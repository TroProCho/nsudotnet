using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberGuesser
{
    class NumberGuesser
    {
        const string NewGameString = "You wanna play a little game?! What's your name?";
        private const string StartGameString = "Game started, {0}. You have to guess my number from {1} to {2}. To exit enter \"q\"";
        private const string ExitAnswer = "q";
        const string ExitGameString = "Sorry, {0}. It's just a game.";
        private const string AnswetNotNumber = "Ho-Ho-Ho. My darling. Try to enter the number next time.";
        static readonly string[] MoralSupport = {"Loser!", "You shall not win!", "LOL! Wrong answer."};
        private const string NumberLess = "Your number is less then my.";
        private const string NumberGreater = "Your number is greater then my.";
        const int MinValue = 0;
        const int MaxValue = 100;
        const int NumberOfAttemts = 4;
        private const int SizeHistory = 1000;
        const string FormatSpantTime = @"hh\:mm\:ss";

        private TextReader _input;
        private TextWriter _output;
        private string _nickname;
        private List<int> _historyAnswer;
        private int _guessesNumber;

        public NumberGuesser(TextReader input, TextWriter output)
        {
            _input = input ?? Console.In;
            _output = output ?? Console.Out;
            _nickname = "user";
            _historyAnswer = null;
            _guessesNumber = 0;
        }

        
        public void StartGame()
        {
            _output.WriteLine(NewGameString);
            while(string.IsNullOrWhiteSpace(_nickname = _input.ReadLine()));
            _historyAnswer = new List<int>(SizeHistory);
            _output.WriteLine(StartGameString, _nickname, MinValue, MaxValue);
            _guessesNumber = (new Random()).Next(MinValue, MaxValue + 1);
            DateTime startTime = DateTime.Now;
            bool isEnd = false;
            while (!isEnd)
            {
                if (_historyAnswer.Count / NumberOfAttemts > 0 && _historyAnswer.Count % NumberOfAttemts == 0)
                {
                    _output.WriteLine(MoralSupport[(new Random()).Next(0, MoralSupport.Count())]);
                }
                string answer = _input.ReadLine();
                int enteredNumber = 0;
                if (int.TryParse(answer, out enteredNumber))
                {
                    if (enteredNumber == _guessesNumber)
                    {
                        isEnd = true;
                        _output.WriteLine("{0} win! Right answer {1}.", _nickname, _guessesNumber);
                        PrintHistory(startTime);
                    }
                    else
                    {
                        _output.WriteLine(enteredNumber < _guessesNumber ? NumberLess : NumberGreater);
                        _historyAnswer.Add(enteredNumber);
                    }
                }
                else
                {
                    if (answer == ExitAnswer)
                    {
                        _output.WriteLine(ExitGameString, _nickname);
                        isEnd = true;
                    }
                    else
                    {
                        _output.WriteLine(AnswetNotNumber);
                    }
                }
            }
            _input.ReadLine();
        }

        public void PrintHistory(DateTime startTime)
        {
            _output.WriteLine("The number of attempts {0}. History your answer:", _historyAnswer.Count);
            foreach (var value in _historyAnswer)
            {
                if (value < _guessesNumber)
                {
                    _output.WriteLine("{0} < {1}", value, _guessesNumber);
                }
                else
                {
                    _output.WriteLine("{0} > {1}", value, _guessesNumber);
                }
            }
            _output.WriteLine("Spent time: {0}", (DateTime.Now - startTime).ToString(FormatSpantTime));
        }
    } 
}