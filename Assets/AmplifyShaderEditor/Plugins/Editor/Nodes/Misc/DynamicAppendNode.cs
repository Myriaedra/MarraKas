// Amplify Shader Editor - Visual Shader Editing Tool
// Copyright (c) Amplify Creations, Lda <info@amplify.pt>
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace AmplifyShaderEditor
{

	public struct AppendData
	{
		public WirePortDataType PortType;
		public int OldPortId;
		public int NewPortId;
		public AppendData( WirePortDataType portType, int oldPortId, int newPortId )
		{
			PortType = portType;
			OldPortId = oldPortId;
			NewPortId = newPortId;
		}
	}

	[Serializable]
	[NodeAttributes( "Append", "Vector Operators", "Append channels to create a new component" )]
	public sealed class DynamicAppendNode : ParentNode
	{
		private const string OutputTypeStr = "Output type";
		private const string OutputFormatStr = "({0}({1}))";

		[SerializeField]
		private WirePortDataType m_selectedOutputType = WirePortDataType.FLOAT4;

		[SerializeField]
		private int m_selectedOutputTypeInt = 2;
		
		private readonly string[] m_outputValueTypes ={ "Vector2",
														"Vector3",
														"Vector4",
														"Color"};

		[SerializeField]
		private int[] m_occupiedChannels = { -1, -1, -1, -1 };

		private readonly string[] m_channelNamesVector = { "X", "Y", "Z", "W" };
		private readonly string[] m_channelNamesColor = { "R", "G", "B", "A" };

		protected override void CommonInit( int uniqueId )
		{
			base.CommonInit( uniqueId );
			AddInputPort( WirePortDataType.FLOAT, false, m_channelNamesVector[ 0 ] );
			AddInputPort( WirePortDataType.FLOAT, false, m_channelNamesVector[ 1 ] );
			AddInputPort( WirePortDataType.FLOAT, false, m_channelNamesVector[ 2 ] );
			AddInputPort( WirePortDataType.FLOAT, false, m_channelNamesVector[ 3 ] );
			AddOutputPort( m_selectedOutputType, Constants.EmptyPortValue );
			m_textLabelWidth = 90;
			m_autoWrapProperties = true;
			m_useInternalPortData = true;
			m_hasLeftDropdown = true;
			m_previewShaderGUID = "d80ac81aabf643848a4eaa76f2f88d65";
		}

		void NewUpdateBehaviorConn( int portId , bool onLoading )
		{
			InputPort inputPort = GetInputPortByUniqueId( portId );
			int channelsRequired = UIUtils.GetChannelsAmount( onLoading?inputPort.DataType:inputPort.ConnectionType( 0 ) );
			int availableChannels = UIUtils.GetChannelsAmount( m_selectedOutputType );

			// Invalidate previously used channels
			for( int i = 0; i < availableChannels; i++ )
			{
				if( m_occupiedChannels[ i ] == portId )
				{
					m_occupiedChannels[ i ] = -1;
					m_inputPorts[ i ].Visible = true;
				}
			}
			// Lock available channels to port
			int len = Mathf.Min( portId + channelsRequired, availableChannels );

			int channelsUsed = 0;
			for( int i = portId; i < len; i++ )
			{
				if( m_occupiedChannels[ i ] == -1 )
				{
					m_occupiedChannels[ i ] = portId;
					channelsUsed += 1;
				}
				else
				{
					break;
				}
			}

			if( !onLoading )
				inputPort.ChangeType( UIUtils.GetWireTypeForChannelAmount( channelsUsed ), false );

			if( channelsUsed > 1 && portId < availableChannels - 1 )
			{
				channelsUsed -= 1;
				int i = portId + 1;
				for( ; channelsUsed > 0; i++, --channelsUsed )
				{
					m_inputPorts[ i ].Visible = false;
				}

			}
			m_sizeIsDirty = true;
		}

		void NewUpdateBehaviorDisconn( int portId )
		{
			int availableChannels = UIUtils.GetChannelsAmount( m_selectedOutputType );
			// Invalidate previously used channels
			for( int i = 0; i < availableChannels; i++ )
			{
				if( m_occupiedChannels[ i ] == portId )
				{
					m_occupiedChannels[ i ] = -1;
					m_inputPorts[ i ].Visible = true;
					m_inputPorts[ i ].ChangeType( WirePortDataType.FLOAT, false );
				}
			}
			m_sizeIsDirty = true;
		}

		void RenamePorts()
		{
			int channel = 0;
			for( int i = 0; i < 4; i++ )
			{
				if( m_inputPorts[ i ].Visible )
				{
					string name = string.Empty;
					int usedChannels = UIUtils.GetChannelsAmount( m_inputPorts[ i ].DataType );
					bool isColor = ( m_selectedOutputType == WirePortDataType.COLOR );
					for( int j = 0; j < usedChannels; j++ )
					{
						if( channel < m_channelNamesVector.Length )
							name += isColor ? m_channelNamesColor[ channel++ ] : m_channelNamesVector[ channel++ ];
					}
					m_inputPorts[ i ].Name = name;
				}
			}
		}

		void UpdatePortTypes()
		{
			ChangeOutputType( m_selectedOutputType, false );
			int availableChannels = UIUtils.GetChannelsAmount( m_selectedOutputType );
			int usedChannels = 0;
			while( usedChannels < availableChannels )
			{
				int channelsRequired = m_inputPorts[ usedChannels ].IsConnected ? UIUtils.GetChannelsAmount( m_inputPorts[ usedChannels ].DataType ) : 0;
				if( channelsRequired > 0 )
				{

					if( ( usedChannels + channelsRequired ) < availableChannels )
					{
						usedChannels += channelsRequired;
					}
					else
					{
						m_inputPorts[ usedChannels ].Visible = true;
						WirePortDataType newType = UIUtils.GetWireTypeForChannelAmount( availableChannels - usedChannels );
						m_inputPorts[ usedChannels ].ChangeType( newType, false );
						usedChannels = availableChannels;
						break;
					}
				}
				else
				{
					m_occupiedChannels[ usedChannels ] = -1;
					m_inputPorts[ usedChannels ].Visible = true;
					m_inputPorts[ usedChannels ].ChangeType( WirePortDataType.FLOAT, false );
					usedChannels += 1;
				}
			}

			for( int i = usedChannels; i < availableChannels; i++ )
			{
				m_occupiedChannels[ i ] = -1;
				m_inputPorts[ i ].Visible = true;
				m_inputPorts[ i ].ChangeType( WirePortDataType.FLOAT, false );
			}

			for( int i = availableChannels; i < 4; i++ )
			{
				m_occupiedChannels[ i ] = -1;
				m_inputPorts[ i ].Visible = false;
				m_inputPorts[ i ].ChangeType( WirePortDataType.FLOAT, false );
			}
			m_sizeIsDirty = true;
		}

		public override void OnInputPortConnected( int portId, int otherNodeId, int otherPortId, bool activateNode = true )
		{
			base.OnInputPortConnected( portId, otherNodeId, otherPortId, activateNode );

			if( m_containerGraph.ParentWindow.IsLoading && UIUtils.CurrentShaderVersion() < 13206 )
				return;

			NewUpdateBehaviorConn( portId , m_containerGraph.ParentWindow.IsLoading );
			RenamePorts();

		}

		public override void OnInputPortDisconnected( int portId )
		{
			base.OnInputPortDisconnected( portId );

			if( m_containerGraph.ParentWindow.IsLoading && UIUtils.CurrentShaderVersion() < 13206 )
				return;

			NewUpdateBehaviorDisconn( portId );
			RenamePorts();
		}

		public override void OnConnectedOutputNodeChanges( int portId, int otherNodeId, int otherPortId, string name, WirePortDataType type )
		{
			base.OnConnectedOutputNodeChanges( portId, otherNodeId, otherPortId, name, type );

			if( m_containerGraph.ParentWindow.IsLoading && UIUtils.CurrentShaderVersion() < 13206 )
				return;

			NewUpdateBehaviorConn( portId, m_containerGraph.ParentWindow.IsLoading );
			RenamePorts();
		}

		void SetupPorts()
		{
			switch( m_selectedOutputTypeInt )
			{
				case 0: m_selectedOutputType = WirePortDataType.FLOAT2; break;
				case 1: m_selectedOutputType = WirePortDataType.FLOAT3; break;
				case 2: m_selectedOutputType = WirePortDataType.FLOAT4; break;
				case 3: m_selectedOutputType = WirePortDataType.COLOR; break;
			}
			UpdatePortTypes();
			RenamePorts();
		}

		public override void Draw( DrawInfo drawInfo )
		{
			base.Draw( drawInfo );

			if( m_dropdownEditing )
			{
				EditorGUI.BeginChangeCheck();
				m_selectedOutputTypeInt = EditorGUIPopup( m_dropdownRect, m_selectedOutputTypeInt, m_outputValueTypes, UIUtils.PropertyPopUp );
				if( EditorGUI.EndChangeCheck() )
				{
					SetupPorts();
					m_dropdownEditing = false;
				}
			}
		}

		public override void DrawProperties()
		{
			base.DrawProperties();
			EditorGUILayout.BeginVertical();

			EditorGUI.BeginChangeCheck();
			m_selectedOutputTypeInt = EditorGUILayoutPopup( OutputTypeStr, m_selectedOutputTypeInt, m_outputValueTypes );
			if( EditorGUI.EndChangeCheck() )
			{
				SetupPorts();
			}

			EditorGUILayout.EndVertical();
		}

		public override string GenerateShaderForOutput( int outputId, ref MasterNodeDataCollector dataCollector, bool ignoreLocalVar )
		{
			if( m_outputPorts[ 0 ].IsLocalValue )
				return m_outputPorts[ 0 ].LocalValue;
			string result = string.Empty;
			for( int i = 0; i < 4; i++ )
			{
				if( m_inputPorts[ i ].Visible )
				{
					if( i > 0 )
					{
						result += " , ";
					}
					result += m_inputPorts[ i ].GeneratePortInstructions( ref dataCollector );
				}
			}

			result = string.Format( OutputFormatStr,
									UIUtils.FinalPrecisionWirePortToCgType( m_currentPrecisionType, m_selectedOutputType ),
									result );

			RegisterLocalVariable( 0, result, ref dataCollector, "appendResult" + OutputId );
			return m_outputPorts[ 0 ].LocalValue;
		}

		public override void ReadFromString( ref string[] nodeParams )
		{
			base.ReadFromString( ref nodeParams );
			m_selectedOutputType = (WirePortDataType)Enum.Parse( typeof( WirePortDataType ), GetCurrentParam( ref nodeParams ) );
			switch( m_selectedOutputType )
			{
				case WirePortDataType.FLOAT2: m_selectedOutputTypeInt = 0; break;
				case WirePortDataType.FLOAT3: m_selectedOutputTypeInt = 1; break;
				case WirePortDataType.FLOAT4: m_selectedOutputTypeInt = 2; break;
				case WirePortDataType.COLOR: m_selectedOutputTypeInt = 3; break;
			}
		}

		public override void ReadFromDeprecated( ref string[] nodeParams, Type oldType = null )
		{
			m_selectedOutputType = (WirePortDataType)Enum.Parse( typeof( WirePortDataType ), GetCurrentParam( ref nodeParams ) );
			switch( m_selectedOutputType )
			{
				case WirePortDataType.FLOAT2: m_selectedOutputTypeInt = 0; break;
				case WirePortDataType.FLOAT3: m_selectedOutputTypeInt = 1; break;
				case WirePortDataType.FLOAT4: m_selectedOutputTypeInt = 2; break;
				case WirePortDataType.COLOR: m_selectedOutputTypeInt = 3; break;
			}
		}

		public override void RefreshExternalReferences()
		{
			base.RefreshExternalReferences();

			if( UIUtils.CurrentShaderVersion() < 13206 )
			{
				//TODO: MAKE THIS LESS BRUTE FORCE
				List<AppendData> reroutes = new List<AppendData>();
				int availableChannel = 0;
				for( int i = 0; i < 4 && availableChannel < 4; i++ )
				{
					int channelsAmount = UIUtils.GetChannelsAmount( m_inputPorts[ i ].DataType );
					if( m_inputPorts[ i ].IsConnected /*&& availableChannel != i*/ )
					{
						reroutes.Add( new AppendData( m_inputPorts[ i ].DataType, i, availableChannel ) );
					}

					availableChannel += channelsAmount;
				}

				if( reroutes.Count > 0 )
				{
					for( int i = reroutes.Count - 1; i > -1; i-- )
					{
						int nodeId = m_inputPorts[ reroutes[ i ].OldPortId ].ExternalReferences[ 0 ].NodeId;
						int portId = m_inputPorts[ reroutes[ i ].OldPortId ].ExternalReferences[ 0 ].PortId;

						m_containerGraph.DeleteConnection( true, UniqueId, reroutes[ i ].OldPortId, false, false, false );
						m_containerGraph.CreateConnection( UniqueId, reroutes[ i ].NewPortId, nodeId, portId, false );
						NewUpdateBehaviorConn( reroutes[ i ].NewPortId , true );
					}
				}

				availableChannel = UIUtils.GetChannelsAmount( m_selectedOutputType );
				int currChannelIdx = 0;
				for( ; currChannelIdx < availableChannel; currChannelIdx++ )
				{
					if( m_inputPorts[ currChannelIdx ].Visible )
					{
						int channelsAmount = UIUtils.GetChannelsAmount( m_inputPorts[ currChannelIdx ].DataType );
						for( int j = currChannelIdx + 1; j < currChannelIdx + channelsAmount; j++ )
						{
							m_inputPorts[ j ].Visible = false;
						}
					}
				}

				for( ; currChannelIdx < 4; currChannelIdx++ )
				{
					m_inputPorts[ currChannelIdx ].Visible = false;
				}
			}
			SetupPorts();
			m_sizeIsDirty = true;
		}

		public override void WriteToString( ref string nodeInfo, ref string connectionsInfo )
		{
			base.WriteToString( ref nodeInfo, ref connectionsInfo );
			IOUtils.AddFieldValueToString( ref nodeInfo, m_selectedOutputType );
		}
	}
}
