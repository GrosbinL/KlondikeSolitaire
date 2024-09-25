/* SeedDialog.cs
 * Author: Grosbin Orellana Luna
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grosbin.Games.KlondikeSolitaire
{
    /// <summary>
    /// A dialog for obtaining a new seed.
    /// </summary>
    public partial class SeedDialog : Form
    {
        /// <summary>
        /// Gets or sets the seed.
        /// </summary>
        public int Seed
        {
            get
            {
                return (int)uxSeed.Value;
            }
            set
            {
                uxSeed.Value = value;
            }
        }

        /// <summary>
        /// Constructs the dialog.
        /// </summary>
        public SeedDialog()
        {
            InitializeComponent();
        }
    }
}
