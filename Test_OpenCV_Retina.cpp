//#include <opencv2/core/core.hpp>
//#include <opencv2/highgui/highgui.hpp>
//#include <opencv2/imgproc/imgproc.hpp>
#include <opencv2/opencv.hpp>
#include <iostream>

using namespace cv;
using namespace std;

int main( int argc, char** argv )
{
	// Declare the retina input buffer... that will be fed differently in regard of the input media
	cv::Mat inputFrame;
	cv::VideoCapture videoCapture; // in case a video media is used, its manager is declared here

	videoCapture.open(0);

	// grab a first frame to check if everything is ok
	videoCapture >> inputFrame;

	if (inputFrame.empty())
		return -1;

	//////////////////////////////////////////////////////////////////////////////
	// Program start in a try/catch safety context (Retina may throw errors)
	try
	{
		// create a retina instance with default parameters setup, uncomment the initialisation you wanna test
		cv::Ptr<cv::Retina> myRetina;

		// Activate log sampling (favour foveal vision and subsamples peripheral vision)
		//myRetina = new cv::Retina(inputFrame.size(), true, cv::RETINA_COLOR_BAYER, true, 2.0, 10.0);
		// Allocate "classical" retina :
		myRetina = new cv::Retina(inputFrame.size());

		// save default retina parameters file in order to let you see this and maybe modify it and reload using method "setup"
		//myRetina->write("RetinaDefaultParameters.xml");

		// load parameters if file exists
		myRetina->setup("RetinaSpecificParameters.xml");
		myRetina->clearBuffers();

		// declare retina output buffers
		cv::Mat retinaOutput_parvo, prev_retinaOutput_parvo;
		cv::Mat retinaOutput_magno, prev_retinaOutput_magno;

		cv::Mat retinaFlow_magno;
		cv::Mat retinaFlow_parvo;

		myRetina->run(inputFrame);
		
		prev_retinaOutput_parvo = retinaOutput_parvo;
		myRetina->getParvo(retinaOutput_parvo);
		
		prev_retinaOutput_magno = retinaOutput_magno;
		myRetina->getMagno(retinaOutput_magno);

		// processing loop with stop condition
		while(true)
		{
			// if using video stream, then, grabbing a new frame, else, input remains the same
			if (videoCapture.isOpened())
				videoCapture>>inputFrame;

			// run retina filter
			myRetina->run(inputFrame);

			// Retrieve and display retina output
			prev_retinaOutput_parvo = retinaOutput_parvo;
			myRetina->getParvo(retinaOutput_parvo);

			prev_retinaOutput_magno = retinaOutput_magno;
			myRetina->getMagno(retinaOutput_magno);

			//GaussianBlur(retinaOutput_magno, retinaOutput_magno, Size(7,7), 1.5, 1.5);
			//Canny(retinaOutput_magno, retinaOutput_magno, 0, 30, 3);

			cv::imshow("retina input", inputFrame);
			cv::imshow("Retina Parvo", retinaOutput_parvo);
			cv::imshow("Retina Magno", retinaOutput_magno);

			//calcOpticalFlowFarneback(prev_retinaOutput_magno, retinaOutput_magno,
			//	retinaFlow_magno, 0.5, 3, 15, 3, 5, 1.2, OPTFLOW_FARNEBACK_GAUSSIAN);
			//cv::imshow("Magno Flow", retinaFlow_magno);

			if (waitKey(30) >= 0)
				break;
		}
	}
	catch(cv::Exception e)
	{
		std::cerr<<"Error using Retina : " << e.what()<<std::endl;
	}

	// Program end message
	std::cout<<"Retina demo end"<<std::endl;

	return 0;
}
