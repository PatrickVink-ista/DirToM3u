using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Fringilla.Media;

internal sealed class NumericExtractRegex : Regex
{
    /// <summary>Cached, thread-safe singleton instance.</summary>
    internal static readonly NumericExtractRegex Instance = new();

    /// <summary>Initializes the instance.</summary>
    private NumericExtractRegex()
    {
        base.pattern = "(\\d+)\\D+(\\d+)";
        base.roptions = RegexOptions.None;
        ValidateMatchTimeout(Utilities.s_defaultTimeout);
        base.internalMatchTimeout = Utilities.s_defaultTimeout;
        base.factory = new RunnerFactory();
        base.capsize = 3;
    }

    /// <summary>Provides a factory for creating <see cref="RegexRunner"/> instances to be used by methods on <see cref="Regex"/>.</summary>
    private sealed class RunnerFactory : RegexRunnerFactory
    {
        /// <summary>Creates an instance of a <see cref="RegexRunner"/> used by methods on <see cref="Regex"/>.</summary>
        protected override RegexRunner CreateInstance() => new Runner();

        /// <summary>Provides the runner that contains the custom logic implementing the specified regular expression.</summary>
        private sealed class Runner : RegexRunner
        {
            /// <summary>Scan the <paramref name="inputSpan"/> starting from base.runtextstart for the next match.</summary>
            /// <param name="inputSpan">The text being scanned by the regular expression.</param>
            protected override void Scan(ReadOnlySpan<char> inputSpan)
            {
                // Search until we can't find a valid starting position, we find a match, or we reach the end of the input.
                while (TryFindNextPossibleStartingPosition(inputSpan) &&
                       !TryMatchAtCurrentPosition(inputSpan) &&
                       base.runtextpos != inputSpan.Length)
                {
                    base.runtextpos++;
                    if (Utilities.s_hasTimeout)
                    {
                        base.CheckTimeout();
                    }
                }
            }

            /// <summary>Search <paramref name="inputSpan"/> starting from base.runtextpos for the next location a match could possibly start.</summary>
            /// <param name="inputSpan">The text being scanned by the regular expression.</param>
            /// <returns>true if a possible match was found; false if no more matches are possible.</returns>
            private bool TryFindNextPossibleStartingPosition(ReadOnlySpan<char> inputSpan)
            {
                int pos = base.runtextpos;

                // Any possible match is at least 3 characters.
                if (pos <= inputSpan.Length - 3)
                {
                    // The pattern begins with a Unicode digit.
                    // Find the next occurrence. If it can't be found, there's no match.
                    int i = Utilities.IndexOfAnyDigit(inputSpan.Slice(pos));
                    if (i >= 0)
                    {
                        base.runtextpos = pos + i;
                        return true;
                    }
                }

                // No match found.
                base.runtextpos = inputSpan.Length;
                return false;
            }

            /// <summary>Determine whether <paramref name="inputSpan"/> at base.runtextpos is a match for the regular expression.</summary>
            /// <param name="inputSpan">The text being scanned by the regular expression.</param>
            /// <returns>true if the regular expression matches at the current position; otherwise, false.</returns>
            private bool TryMatchAtCurrentPosition(ReadOnlySpan<char> inputSpan)
            {
                int pos = base.runtextpos;
                int matchStart = pos;
                int capture_starting_pos = 0;
                int capture_starting_pos1 = 0;
                int charloop_capture_pos = 0;
                int charloop_capture_pos1 = 0;
                int charloop_starting_pos = 0, charloop_ending_pos = 0;
                int charloop_starting_pos1 = 0, charloop_ending_pos1 = 0;
                ReadOnlySpan<char> slice = inputSpan.Slice(pos);

                // 1st capture group.
                //{
                capture_starting_pos = pos;

                // Match a Unicode digit greedily at least once.
                //{
                charloop_starting_pos = pos;

                int iteration = 0;
                while ((uint)iteration < (uint)slice.Length && char.IsDigit(slice[iteration]))
                {
                    iteration++;
                }

                if (iteration == 0)
                {
                    UncaptureUntil(0);
                    return false; // The input didn't match.
                }

                slice = slice.Slice(iteration);
                pos += iteration;

                charloop_ending_pos = pos;
                charloop_starting_pos++;
                goto CharLoopEnd;

            CharLoopBacktrack:
                UncaptureUntil(charloop_capture_pos);

                if (Utilities.s_hasTimeout)
                {
                    base.CheckTimeout();
                }

                if (charloop_starting_pos >= charloop_ending_pos)
                {
                    UncaptureUntil(0);
                    return false; // The input didn't match.
                }
                pos = --charloop_ending_pos;
                slice = inputSpan.Slice(pos);

            CharLoopEnd:
                charloop_capture_pos = base.Crawlpos();
                //}

                base.Capture(1, capture_starting_pos, pos);

                goto CaptureSkipBacktrack;

            CaptureBacktrack:
                goto CharLoopBacktrack;

            CaptureSkipBacktrack:;
                //}

                // Match any character other than a Unicode digit greedily at least once.
                //{
                charloop_starting_pos1 = pos;

                int iteration1 = 0;
                while ((uint)iteration1 < (uint)slice.Length && !char.IsDigit(slice[iteration1]))
                {
                    iteration1++;
                }

                if (iteration1 == 0)
                {
                    goto CaptureBacktrack;
                }

                slice = slice.Slice(iteration1);
                pos += iteration1;

                charloop_ending_pos1 = pos;
                charloop_starting_pos1++;
                goto CharLoopEnd1;

            CharLoopBacktrack1:
                UncaptureUntil(charloop_capture_pos1);

                if (Utilities.s_hasTimeout)
                {
                    base.CheckTimeout();
                }

                if (charloop_starting_pos1 >= charloop_ending_pos1)
                {
                    goto CaptureBacktrack;
                }
                pos = --charloop_ending_pos1;
                slice = inputSpan.Slice(pos);

            CharLoopEnd1:
                charloop_capture_pos1 = base.Crawlpos();
                //}

                // 2nd capture group.
                {
                    capture_starting_pos1 = pos;

                    // Match a Unicode digit atomically at least once.
                    {
                        int iteration2 = 0;
                        while ((uint)iteration2 < (uint)slice.Length && char.IsDigit(slice[iteration2]))
                        {
                            iteration2++;
                        }

                        if (iteration2 == 0)
                        {
                            goto CharLoopBacktrack1;
                        }

                        slice = slice.Slice(iteration2);
                        pos += iteration2;
                    }

                    base.Capture(2, capture_starting_pos1, pos);
                }

                // The input matched.
                base.runtextpos = pos;
                base.Capture(0, matchStart, pos);
                return true;

                // <summary>Undo captures until it reaches the specified capture position.</summary>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                void UncaptureUntil(int capturePosition)
                {
                    while (base.Crawlpos() > capturePosition)
                    {
                        base.Uncapture();
                    }
                }
            }
        }
    }

    private static class Utilities
    {
        /// <summary>Default timeout value set in <see cref="AppContext"/>, or <see cref="Regex.InfiniteMatchTimeout"/> if none was set.</summary>
        internal static readonly TimeSpan s_defaultTimeout = AppContext.GetData("REGEX_DEFAULT_MATCH_TIMEOUT") is TimeSpan timeout ? timeout : Regex.InfiniteMatchTimeout;

        /// <summary>Whether <see cref="s_defaultTimeout"/> is non-infinite.</summary>
        internal static readonly bool s_hasTimeout = s_defaultTimeout != Regex.InfiniteMatchTimeout;

        /// <summary>Finds the next index of any character that matches a Unicode digit.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int IndexOfAnyDigit(ReadOnlySpan<char> span)
        {
            int i = span.IndexOfAnyExcept(Utilities.s_asciiExceptDigits);
            if ((uint)i < (uint)span.Length)
            {
                if (char.IsAscii(span[i]))
                {
                    return i;
                }

                do
                {
                    if (char.IsDigit(span[i]))
                    {
                        return i;
                    }
                    i++;
                }
                while ((uint)i < (uint)span.Length);
            }

            return -1;
        }

        /// <summary>Supports searching for characters in or not in "\0\u0001\u0002\u0003\u0004\u0005\u0006\a\b\t\n\v\f\r\u000e\u000f\u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019\u001a\u001b\u001c\u001d\u001e\u001f !\"#$%&amp;'()*+,-./:;&lt;=&gt;?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~\u007f".</summary>
        internal static readonly SearchValues<char> s_asciiExceptDigits = SearchValues.Create("\0\u0001\u0002\u0003\u0004\u0005\u0006\a\b\t\n\v\f\r\u000e\u000f\u0010\u0011\u0012\u0013\u0014\u0015\u0016\u0017\u0018\u0019\u001a\u001b\u001c\u001d\u001e\u001f !\"#$%&'()*+,-./:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~\u007f");
    }
}
