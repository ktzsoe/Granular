﻿using System;
using System.Collections.Generic;
using Bridge.Html5;
using System.Linq;
using System.Text;
using Granular.Extensions;

namespace Granular.Host.Render
{
    public class HtmlRenderElement
    {
        public HTMLElement HtmlElement { get; private set; }

        public HtmlRenderElement() :
            this(Document.CreateElement("div"))
        {
            //
        }

        public HtmlRenderElement(HTMLElement htmlElement)
        {
            this.HtmlElement = htmlElement;
        }
    }
}
