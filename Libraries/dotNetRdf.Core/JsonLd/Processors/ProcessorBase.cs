/*
// <copyright>
// dotNetRDF is free and open source software licensed under the MIT License
// -------------------------------------------------------------------------
// 
// Copyright (c) 2009-2024 dotNetRDF Project (http://dotnetrdf.org/)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is furnished
// to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
*/

using System.Collections.Generic;
using VDS.RDF.JsonLd.Syntax;

namespace VDS.RDF.JsonLd.Processors
{
    /// <summary>
    /// Base class for processors that make use of <see cref="JsonLdProcessorOptions"/>.
    /// </summary>
    internal class ProcessorBase
    {
        /// <summary>
        /// Get the options passed to this processor in the constructor.
        /// </summary>
        protected JsonLdProcessorOptions Options { get; }

        /// <summary>
        /// Get the list of warnings generated by this processor.
        /// </summary>
        public IList<JsonLdProcessorWarning> Warnings { get; }

        /// <summary>
        /// Create a new processor instance that uses the specified processor options.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="warnings">The list to add any generated warnings to.</param>
        public ProcessorBase(JsonLdProcessorOptions options, IList<JsonLdProcessorWarning> warnings)
        {
            Options = options;
            Warnings = warnings;
        }

        /// <summary>
        /// Raise a warning from this processor.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        protected void Warn(JsonLdErrorCode errorCode, string message)
        {
            Warnings.Add(new JsonLdProcessorWarning(errorCode, message));
        }

        /// <summary>
        /// If the processing mode is <see cref="JsonLdProcessingMode.JsonLd10"/>, throw a <see cref="JsonLdProcessorException"/> with error code
        /// <see cref="JsonLdErrorCode.ProcessingModeConflict"/>.
        /// </summary>
        /// <param name="keyword">The keyword that caused the check.</param>
        /// <param name="errorCode">The error code to use when raising a JsonLdProcessorException if the processing mode is <see cref="JsonLdProcessingMode.JsonLd10"/>.</param>
        protected void CheckProcessingMode(string keyword,
            JsonLdErrorCode errorCode = JsonLdErrorCode.ProcessingModeConflict)
        {
            if (Options.ProcessingMode == JsonLdProcessingMode.JsonLd10)
            {
                throw new JsonLdProcessorException(errorCode,
                    $"Processing mode conflict. Processor options specify JSON-LD 1.0 processing mode, but encountered {keyword} that requires JSON-LD 1.1 processing features");
            }
        }
    }
}