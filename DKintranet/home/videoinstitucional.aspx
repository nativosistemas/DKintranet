<%@ Page Title="" Language="C#" MasterPageFile="~/master/home.Master" AutoEventWireup="true" CodeBehind="videoinstitucional.aspx.cs" Inherits="DKintranet.home.videoinstitucional" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript"  src="../includes/home/js/jquery/modernizr.js"></script>
    	<script type="text/javascript" src="../includes/home/js/jquery/timeline.js"></script>
	<script type="text/javascript">
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
        #vid {
            width:100%;
        }
        @media (min-width: 768px) {
            #vid {
                width: 80%;
                margin-left: 10%;
            }
        }
    </style>
    <section class="section-gris-claro">
        <video controls="controls" autoplay="autoplay" loop="loop" id="vid" muted="muted">
          <source src="/archivos/video_institucional.MP4" type="video/mp4" />
          Your browser does not support the video tag.
        </video>
    </section>
    <div class="clear100 visible-lg "></div>
    <div class="clear30 visible-md"></div>

    <script>
        $(document).ready(function () {
            setTimeout(function () {
                $("#vid").removeAttr("muted");
            }, 1000)
        });

        function toggleMute() {

            var video = document.getElementById("vid");

            if (video.muted) {
                video.muted = false;
            } else {
                debugger;
                video.muted = true;
                video.play()
            }

        }
    </script>

</asp:Content>

