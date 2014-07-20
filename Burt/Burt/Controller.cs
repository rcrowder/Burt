using System;
using Microsoft.SPOT;

namespace Burt
{
    class Controller
    {
        abstract class Position
        {
    	    public abstract string Title { get; }
        }

        class Manager : Position
        {
	        public override string Title
	        {
	            get
	            {
		        return "Manager";
	            }
	        }
        }

        class Clerk : Position
        {
	        public override string Title
	        {
	            get
	            {
		        return "Clerk";
	            }
	        }
        }

        class Programmer : Position
        {
	        public override string Title
	        {
	            get
	            {
		        return "Programmer";
	            }
	        }
        }

        static class ControllerFactory
        {
	        /// <summary>
	        /// Decides which class to instantiate.
	        /// </summary>
	        public static Position Get(int id)
	        {
	            switch (id)
	            {
		        case 0:
		            return new Manager();
		        case 1:
		        case 2:
		            return new Clerk();
		        case 3:
		        default:
		            return new Programmer();
	            }
	        }
        }
    }
}
